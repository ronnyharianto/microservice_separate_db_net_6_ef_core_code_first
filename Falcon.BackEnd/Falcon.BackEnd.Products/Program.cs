using Falcon.BackEnd.Products.Controllers.Products.CustomModels;
using Falcon.BackEnd.Products.Domain;
using Falcon.BackEnd.Products.Service.Products;
using Falcon.Libraries.Microservice.Startups;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder();

builder.UseMicroservice<ApplicationDbContext>()
	   .UseRedis();

builder.Services.AddScoped<ProductService>();

var app = builder.Build();

app.RunMicroservice<ApplicationDbContext>();
