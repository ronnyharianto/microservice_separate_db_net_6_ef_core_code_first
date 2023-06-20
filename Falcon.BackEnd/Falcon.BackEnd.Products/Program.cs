using Falcon.BackEnd.Products.Controllers.Products.CustomModels;
using Falcon.BackEnd.Products.Domain;
using Falcon.BackEnd.Products.Service.Products;
using Falcon.Libraries.Microservice.Startups;
using Microsoft.Extensions.Configuration;

var app = new Startup<ApplicationDbContext>(args);

app.Builder.Services.AddScoped<ProductService>();

// Configure strongly typed settings objects
var appSettingsSection = app.Builder.Configuration.GetSection("FcmNotification");
app.Builder.Services.Configure<FcmNotificationSetting>(appSettingsSection);

app.Run();
