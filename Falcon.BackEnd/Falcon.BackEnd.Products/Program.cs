using Falcon.BackEnd.Products.Controllers.Products.CustomModels;
using Falcon.BackEnd.Products.Domain;
using Falcon.BackEnd.Products.Service.Products;
using Falcon.Libraries.Microservice.Startups;
using Microsoft.Extensions.Configuration;

var app = new Startup<ApplicationDbContext>(args);

app.Builder.Services.AddScoped<ProductService>();

app.Run();
