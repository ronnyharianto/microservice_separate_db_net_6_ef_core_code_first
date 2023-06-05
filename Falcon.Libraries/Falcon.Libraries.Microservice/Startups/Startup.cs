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
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace Falcon.Libraries.Microservice.Startups
{
    public class Startup<TApplicationDbContext> where TApplicationDbContext : DbContext
    {
        public Startup(string[] args)
        {
            var callingAssembly = Assembly.GetCallingAssembly();
            Builder = WebApplication.CreateBuilder(args);

            ConfigureGeneral();
            ConfigureController();
            ConfigureDbContext();
            ConfigureFluentValidation(callingAssembly);
            ConfigureKafka(callingAssembly);
            ConfigureAutoMapper(callingAssembly);
            ConfigureHttpClient();
            ConfigureRedis();
            ConfigureLogging(callingAssembly);
        }
        public WebApplicationBuilder Builder { get; set; }

        public void Run()
        {
            var app = Builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<TApplicationDbContext>();

                if (app.Environment.IsProduction())
                {
                    dbContext.Database.Migrate();
                }
                else
                {
                    dbContext.Database.EnsureCreated();
                }
            }

            app.UseHttpLogging().UseSerilogRequestLogging();

            app.MapControllers();

            app.Run();
        }

        #region Private Methods
        private void ConfigureGeneral()
        {
            Builder.Services.AddScoped<JsonHelper>();
        }

        private void ConfigureController()
        {
            // Add services to the container.
            Builder.Services.AddControllers(
                options =>
                {
                    //Add transaction filter to apply transaction scope for each request on controller
                    options.Filters.Add<TransactionFilterAttribute<TApplicationDbContext>>();
                })
                .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        private void ConfigureDbContext()
        {
            // Add db context
            Builder.Services.AddDbContext<TApplicationDbContext>(
                options => options.UseNpgsql(Builder.Configuration.GetConnectionString("postgreSQL"))
            );
        }

        private void ConfigureFluentValidation(Assembly callingAssembly)
        {
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
                    Builder.Services.AddScoped(genericValidatorType, validator);
                }
            }

            // Run validation using fluentvalidation every request in controller
            Builder.Services.AddFluentValidationAutoValidation();
        }

        private void ConfigureKafka(Assembly callingAssembly)
        {
            var kafkaServer = Builder.Configuration.GetConnectionString("kafka");

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
                        Builder.Services.AddTransient(genericValidatorType, handler);
                    }
                }

                Builder.Services.AddCap(capConfig =>
                {
                    capConfig.UseEntityFramework<TApplicationDbContext>();

                    capConfig.UseKafka(kafkaServer);

                    capConfig.UseDashboard();
                }).AddSubscribeFilter<TransactionSubscribeFilter<TApplicationDbContext>>();
            }
        }

        private void ConfigureAutoMapper(Assembly callingAssembly)
        {
            Builder.Services.AddAutoMapper(callingAssembly);
        }

        private void ConfigureHttpClient()
        {
            Builder.Services.AddHttpContextAccessor();
            Builder.Services.AddHttpClient<HttpClientHelper>();
        }

        private void ConfigureRedis()
        {
            Builder.Services.AddStackExchangeRedisCache(option =>
            {
                option.Configuration = Builder.Configuration.GetConnectionString("redis");
                option.InstanceName = "Falcon-Redis";
            });

            Builder.Services.AddScoped<CacheHelper>();
        }

        private void ConfigureLogging(Assembly callingAssembly)
        {
            var elasticUri = Builder.Configuration.GetConnectionString("elasticsearch");
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

            Builder.Services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.RequestBody | HttpLoggingFields.ResponseBody;

                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;
            });

            Builder.Host.UseSerilog();
        }
        #endregion
    }
}
