using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;

namespace Falcon.Libraries.Microservice
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

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            Builder.Services.AddEndpointsApiExplorer();
            Builder.Services.AddSwaggerGen();
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

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
