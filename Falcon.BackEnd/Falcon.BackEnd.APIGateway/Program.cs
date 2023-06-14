using Falcon.Libraries.Security.JwtToken;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

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

            app.UseOcelot().Wait();

            app.Run();
        }
    }
}