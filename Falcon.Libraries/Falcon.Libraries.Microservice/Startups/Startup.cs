using Falcon.Libraries.Microservice.Controllers;
using FluentValidation;
using FluentValidation.AspNetCore;
using KafkaFlow;
using KafkaFlow.Configuration;
using KafkaFlow.Serializer;
using KafkaFlow.TypedHandler;
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

            try
            {
                var kafkaBus = app.Services.CreateKafkaBus();
                kafkaBus.StartAsync();
            }
            catch { }

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
                options => options.UseNpgsql(Builder.Configuration.GetConnectionString("Default"))
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
            Action<IKafkaConfigurationBuilder> kafkaConfiguration =
                kafka => kafka.AddCluster(
                    cluster => cluster.WithBrokers(new[] { "127.0.0.100:9092" })
                        .AddProducer(
                            "general-producer",
                            producer => producer.DefaultTopic("general-topic")
                                .WithAcks(Acks.All)
                                .AddMiddlewares(m => m.AddSerializer<JsonCoreSerializer>())
                        )
                );

            var handlers = callingAssembly.GetTypes()
                                .Where(x => !x.IsAbstract && !x.IsInterface && typeof(IMessageHandler).IsAssignableFrom(x))
                                .ToList();

            foreach (var handler in handlers)
            {
                var baseType = ((Type[])((TypeInfo)handler).ImplementedInterfaces)[0];
                var genericArgsBaseType = baseType?.GetGenericArguments().FirstOrDefault();
                if (genericArgsBaseType != null)
                {
                    var topicName = genericArgsBaseType.Name;

                    kafkaConfiguration +=
                        kafka => kafka.AddCluster(
                            cluster => cluster.WithBrokers(new[] { "127.0.0.100:9092" })
                                .AddConsumer(
                                    consumer => consumer.Topic(topicName)
                                        .WithGroupId("group-id")
                                        .WithWorkersCount(1)
                                        .WithBufferSize(100)
                                        .AddMiddlewares(middlewares => middlewares
                                            .AddSerializer<JsonCoreSerializer>()
                                            .AddTypedHandlers(h => h.AddHandlers(new List<Type> { handler }))
                                        )
                                )
                        );
                }
            }

            Builder.Services.AddKafka(kafkaConfiguration);
        }

        private void ConfigureAutoMapper(Assembly callingAssembly)
        {
            Builder.Services.AddAutoMapper(callingAssembly);
        }
        #endregion
    }
}
