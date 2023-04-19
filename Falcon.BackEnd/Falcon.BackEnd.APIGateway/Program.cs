using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Falcon.Libraries.Security.JwtToken;

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
                .AddJwtBearer("TestKey", JwtTokenOption.OptionValidation);

            var app = builder.Build();

            app.UseAuthentication();
            app.UseOcelot().Wait();

            app.Run();
        }
    }
}