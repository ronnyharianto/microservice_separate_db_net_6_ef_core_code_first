using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;

namespace Falcon.Libraries.Microservice.Startups
{
    public class Startup<TApplicationDbContext> where TApplicationDbContext : DbContext
    {
        public Startup(string[] args)
        {
            Builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            Builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            Builder.Services.AddDbContext<TApplicationDbContext>(
                options => options.UseNpgsql(Builder.Configuration.GetConnectionString("Default"))
            );
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
