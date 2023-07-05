using Falcon.BackEnd.APIGateway.loggingmiddleware;
using Falcon.Libraries.Microservice.Startups;

var builder = WebApplication.CreateBuilder();

builder.UseApiGatewayService();

var app = builder.Build();

app.RunApiGateway<RequestResponseLoggingMiddleware>();