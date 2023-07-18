using Falcon.BackEnd.APIGateway.Controllers.APIGateway.CustomModels;
using Falcon.BackEnd.APIGateway.Domain;
using Falcon.BackEnd.APIGateway.loggingmiddleware;
using Falcon.BackEnd.APIGateway.Service.APIGateway;
using Falcon.Libraries.Microservice.Startups;

var builder = WebApplication.CreateBuilder();

builder.UseApiGatewayService<ApplicationDbContext>();

builder.Services.AddScoped<APIGatewayService>();

// Configure strongly typed settings objects
var appSettingsSection = builder.Configuration.GetSection("authorizations");
builder.Services.Configure<APIGatewaySetting>(appSettingsSection);


var app = builder.Build();

app.RunApiGateway<RequestResponseLoggingMiddleware, ApplicationDbContext>();