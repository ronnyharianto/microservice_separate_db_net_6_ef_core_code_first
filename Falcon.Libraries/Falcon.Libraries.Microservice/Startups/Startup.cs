using Falcon.Libraries.Microservice.Controllers;
using Falcon.Libraries.Microservice.Subscriber;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Falcon.Libraries.Microservice.Startups
{
    public class Startup<TApplicationDbContext> where TApplicationDbContext : DbContext
    {
        public Startup(string[] args)
        {
            var callingAssembly = Assembly.GetCallingAssembly();
            Builder = WebApplication.CreateBuilder(args);

            ConfigureController();
            ConfigureDbContext();
            ConfigureFluentValidation(callingAssembly);
            ConfigureKafka(callingAssembly);
            ConfigureAutoMapper(callingAssembly);
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

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        #region Private Methods
        private void ConfigureController()
        {
            // Add services to the container.
            Builder.Services.AddControllers(
                options => {
                    //Add transaction filter to apply transaction scope for each request on controller
                    options.Filters.Add<TransactionFilterAttribute<TApplicationDbContext>>();
                })
                .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
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
                }).AddSubscribeFilter<TransactionSubscribeFilter<TApplicationDbContext>>();
            }
        }

        private void ConfigureAutoMapper(Assembly callingAssembly)
        {
            Builder.Services.AddAutoMapper(callingAssembly);
        }
        #endregion
    }
}
