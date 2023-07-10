namespace Falcon.BackEnd.Notifications.Controllers.Notifications.Inputs
{
    public class NotificationTemplateUpdate
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}
