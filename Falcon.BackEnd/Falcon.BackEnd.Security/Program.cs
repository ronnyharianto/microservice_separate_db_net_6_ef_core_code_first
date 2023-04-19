using Falcon.BackEnd.Security.Domain;
using Falcon.Libraries.Microservice;

var app = new Startup<ApplicationDbContext>(args);

app.Run();
