using Falcon.BackEnd.Showcase.Domain;
using Falcon.Libraries.Microservice.Startups;

var builder = WebApplication.CreateBuilder();

builder.UseMicroservice<ApplicationDbContext>()
       .UseLogging();

var app = builder.Build();

app.RunMicroservice<ApplicationDbContext>();