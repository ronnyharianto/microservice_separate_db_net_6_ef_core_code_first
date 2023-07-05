using Falcon.Libraries.Common.Helper;
using Falcon.Libraries.Microservice.Controllers;
using Falcon.Libraries.Microservice.Subscriber;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Sinks.Elasticsearch;
using Serilog;
using System.Reflection;
using Serilog.Exceptions;
using Ocelot.DependencyInjection;
using Falcon.Libraries.Security.JwtToken;

namespace Falcon.Libraries.Microservice.Startups
{
	public static class WebApplicationBuilderExtensions
	{
		public static WebApplicationBuilder UseMicroservice<TApplicationDbContext>(this WebApplicationBuilder builder)
			where TApplicationDbContext : DbContext
		{
			var callingAssembly = Assembly.GetCallingAssembly();

			#region Configure General
			builder.Services.AddScoped<JsonHelper>();
			builder.Services.AddScoped<FirebaseNotificationHelper>();
			#endregion

			#region Configure Controller
			// Add services to the container.
			builder.Services.AddControllers(
				options =>
				{
					//Add transaction filter to apply transaction scope for each request on controller
					options.Filters.Add<TransactionFilterAttribute<TApplicationDbContext>>();
				})
				.AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
			#endregion

			#region Configure Db Context
			// Add db context
			builder.Services.AddDbContext<TApplicationDbContext>(
				options => options.UseNpgsql(builder.Configuration.GetConnectionString("postgreSQL"))
			);
			#endregion

			#region Configure Fluent Validation
			// Register all validator to the service container
			var validators = callingAssembly
									 .GetTypes()
									 .Where(x => !x.IsAbstract && !x.IsInterface && typeof(IValidator).IsAssignableFrom(x))
									 .ToList();

			foreach (var validator in validators)
			{
				var baseType = validator.BaseType;
				var genericArgsBaseType = baseType?.GetGenericArguments().FirstOrDefault();

				if (genericArgsBaseType != null)
				{
					var genericValidatorType = typeof(IValidator<>).MakeGenericType(genericArgsBaseType);
					builder.Services.AddScoped(genericValidatorType, validator);
				}
			}

			// Run validation using fluentvalidation every request in controller
			builder.Services.AddFluentValidationAutoValidation();
			#endregion

			#region Configure Kafka
			var kafkaServer = builder.Configuration.GetConnectionString("kafka");

			if (kafkaServer != null)
			{
				// Register all handler to the service container
				var handlers = callingAssembly
									.GetTypes()
									.Where(x => !x.IsAbstract && !x.IsInterface && x.GetInterfaces().Any(i =>
										i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISubsriberHandler<>))
									)
									.ToList();

				foreach (var handler in handlers)
				{
					var baseType = ((Type[])((TypeInfo)handler).ImplementedInterfaces)[0];
					var genericArgsBaseType = baseType?.GetGenericArguments().FirstOrDefault();

					if (genericArgsBaseType != null)
					{
						var genericValidatorType = typeof(ISubsriberHandler<>).MakeGenericType(genericArgsBaseType);
						builder.Services.AddTransient(genericValidatorType, handler);
					}
				}

				builder.Services.AddCap(capConfig =>
				{
					capConfig.UseEntityFramework<TApplicationDbContext>();

					capConfig.UseKafka(kafkaServer);

					capConfig.UseDashboard();
				}).AddSubscribeFilter<TransactionSubscribeFilter<TApplicationDbContext>>();
			}
			#endregion

			#region Configure Auto Mapper
			builder.Services.AddAutoMapper(callingAssembly);
			#endregion

			#region Configure Http Client
			builder.Services.AddHttpContextAccessor();
			builder.Services.AddHttpClient<HttpClientHelper>();
			#endregion

			return builder;
		}

		public static WebApplicationBuilder UseRedis(this WebApplicationBuilder builder)
		{
			builder.Services.AddStackExchangeRedisCache(option =>
			{
				option.Configuration = builder.Configuration.GetConnectionString("redis");
				option.InstanceName = "Falcon-Redis";
			});

			builder.Services.AddScoped<CacheHelper>();

			return builder;
		}

        public static WebApplicationBuilder UseLogging(this WebApplicationBuilder builder)
        {
            var callingAssembly = Assembly.GetCallingAssembly();

            var elasticUri = builder.Configuration.GetConnectionString("elasticsearch");
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var loggerConfig = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .ReadFrom.Configuration(configuration)
                .WriteTo.Console();

            if (elasticUri != null)
            {
                var callingAssemblyName = callingAssembly.GetName().Name ?? "Unknown";

                loggerConfig.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = $"{callingAssemblyName.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
                });
            }

            Log.Logger = loggerConfig.CreateLogger();

            builder.Services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.RequestBody | HttpLoggingFields.ResponseBody;

                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;
            });

            builder.Host.UseSerilog();

            return builder;
        }

        public static WebApplicationBuilder UseApiGatewayService(this WebApplicationBuilder builder)
        {
            var callingAssembly = Assembly.GetCallingAssembly();

			#region Configure Logging
			var elasticUri = builder.Configuration.GetConnectionString("elasticsearch");
			var configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.Build();

			var loggerConfig = new LoggerConfiguration()
				.Enrich.FromLogContext()
				.Enrich.WithExceptionDetails()
				.ReadFrom.Configuration(configuration)
				.WriteTo.Console();

			if (elasticUri != null)
			{
				var callingAssemblyName = callingAssembly.GetName().Name ?? "Unknown";

				loggerConfig.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri))
				{
					AutoRegisterTemplate = true,
					IndexFormat = $"{callingAssemblyName.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
				});
			}

			Log.Logger = loggerConfig.CreateLogger();

			builder.Host.UseSerilog();
			#endregion

			#region Ocelot
			builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
            builder.Services.AddOcelot(builder.Configuration);
            #endregion

            #region Jwt Bearer
            builder.Services.AddAuthentication()
                .AddJwtBearer("jwt-schema", JwtTokenOption.OptionValidation);
            #endregion

            #region Cors
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("https://localhost:7000")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            #endregion

            return builder;
		}
	}
}
