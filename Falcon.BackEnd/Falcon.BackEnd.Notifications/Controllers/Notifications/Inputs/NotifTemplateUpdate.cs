namespace Falcon.BackEnd.Notifications.Controllers.Notifications.Inputs
{
    public class NotifTemplateUpdate
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}
