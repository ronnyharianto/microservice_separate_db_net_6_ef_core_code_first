using Falcon.BackEnd.Security.Domain;
using Falcon.Libraries.Microservice.Startups;

var app = new Startup<ApplicationDbContext>(args);

app.Run();
