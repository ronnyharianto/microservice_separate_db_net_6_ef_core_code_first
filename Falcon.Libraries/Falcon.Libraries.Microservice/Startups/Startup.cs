using Falcon.Libraries.Microservice.Controllers;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Falcon.Libraries.Microservice.Startups
{
    public class Startup<TApplicationDbContext> where TApplicationDbContext : DbContext
    {
        public Startup(string[] args)
        {
            Builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            Builder.Services.AddControllers(o =>
                {
                    //Add transaction filter to apply transaction scope for each request on controller
                    o.Filters.Add<TransactionFilterAttribute<TApplicationDbContext>>();
                })
                .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
                //.AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

            // Add db context
            Builder.Services.AddDbContext<TApplicationDbContext>(
                options => options.UseNpgsql(Builder.Configuration.GetConnectionString("Default"))
            );

            // Add auto validation using fluentvalidation
            Builder.Services.AddFluentValidationAutoValidation();

            var callingAssembly = Assembly.GetCallingAssembly();
            var validators = callingAssembly.GetTypes()
                                     .Where(x => !x.IsAbstract && !x.IsInterface && typeof(IValidator).IsAssignableFrom(x))
                                     .ToList();

            foreach (var validator in validators)
            {
                var validatorType = validator.BaseType;
                var interfaceType = validatorType?.GetGenericArguments().FirstOrDefault();

                if (interfaceType != null)
                {
                    var genericValidatorType = typeof(IValidator<>).MakeGenericType(interfaceType);
                    Builder.Services.AddScoped(genericValidatorType, validator);
                }
            }
        }

        public WebApplicationBuilder Builder { get; set; }

        public void Run()
        {
            var app = Builder.Build();

            // Migrate latest database changes during startup
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider
                    .GetRequiredService<TApplicationDbContext>();

                //dbContext.Database.Migrate();
                dbContext.Database.EnsureCreated();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
