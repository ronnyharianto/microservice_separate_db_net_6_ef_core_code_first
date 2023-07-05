using Falcon.BackEnd.Notifications.Controllers.Notifications.CustomModels;
using Falcon.BackEnd.Notifications.Domain;
using Falcon.BackEnd.Notifications.Service.Notifications;
using Falcon.Libraries.Microservice.Startups;

var builder = WebApplication.CreateBuilder();

builder.UseMicroservice<ApplicationDbContext>()
	   .UseLogging()
	   .UseRedis();

builder.Services.AddScoped<NotificationService>();

// Configure strongly typed settings objects
var appSettingsSection = builder.Configuration.GetSection("FcmNotification");
builder.Services.Configure<FcmNotificationSetting>(appSettingsSection);

var app = builder.Build();

app.RunMicroservice<ApplicationDbContext>();