using Falcon.BackEnd.Showcase.Domain;
using Falcon.BackEnd.Showcase.Handlers;
using Falcon.Libraries.Microservice.Startups;

var app = new Startup<ApplicationDbContext>(args);

app.Builder.Services.AddTransient<ISubscribeHandler, ProductCreatedHandler>();

app.Run();