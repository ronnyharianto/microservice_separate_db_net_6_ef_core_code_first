using Falcon.BackEnd.Products.Domain;
using Falcon.BackEnd.Products.Service.Products;
using Falcon.Libraries.Microservice.Startups;

var builder = WebApplication.CreateBuilder();

builder.UseMicroservice<ApplicationDbContext>()
	   .UseLogging()
	   .UseRedis();

builder.Services.AddScoped<ProductService>();

var app = builder.Build();

app.RunMicroservice<ApplicationDbContext>();
