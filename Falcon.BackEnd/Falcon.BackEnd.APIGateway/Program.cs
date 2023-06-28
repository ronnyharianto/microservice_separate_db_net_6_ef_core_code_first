using Falcon.BackEnd.APIGateway.loggingmiddleware;
using Falcon.Libraries.Security.JwtToken;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;

namespace Falcon.BackEnd.APIGateway
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
            builder.Services.AddOcelot(builder.Configuration);

            builder.Services.AddAuthentication()
                .AddJwtBearer("jwt-schema", JwtTokenOption.OptionValidation);

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("https://localhost:7000")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            app.UseAuthentication();

            app.UseCors();

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                // Add more Serilog configuration as needed
                .CreateLogger();

            // Register the custom logging middleware before Ocelot is configured
            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            app.UseOcelot().Wait();

            app.Run();
        }
    }
}