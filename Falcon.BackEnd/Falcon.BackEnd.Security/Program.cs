using Falcon.BackEnd.Security.Domain;
using Falcon.Libraries.Microservice.Startups;

var builder = WebApplication.CreateBuilder();

builder.UseMicroservice<ApplicationDbContext>();

var app = builder.Build();

app.RunMicroservice<ApplicationDbContext>();