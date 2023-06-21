using Falcon.BackEnd.Notifications.Controllers.Notifications.CustomModels;
using Falcon.BackEnd.Notifications.Domain;
using Falcon.BackEnd.Notifications.Service.Notifications;
using Falcon.Libraries.Microservice.Startups;

var app = new Startup<ApplicationDbContext>(args);

app.Builder.Services.AddScoped<NotificationService>();

// Configure strongly typed settings objects
var appSettingsSection = app.Builder.Configuration.GetSection("FcmNotification");
app.Builder.Services.Configure<FcmNotificationSetting>(appSettingsSection);

app.Run();
