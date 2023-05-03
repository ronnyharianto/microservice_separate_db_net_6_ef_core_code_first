using Falcon.BackEnd.Showcase.Domain;
using Falcon.Libraries.Microservice.Startups;

var app = new Startup<ApplicationDbContext>(args);

app.Run();