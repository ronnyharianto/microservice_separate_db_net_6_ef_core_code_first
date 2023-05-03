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

            var app = builder.Build();

            app.UseAuthentication();
            app.UseOcelot().Wait();

            app.Run();
        }
    }
}