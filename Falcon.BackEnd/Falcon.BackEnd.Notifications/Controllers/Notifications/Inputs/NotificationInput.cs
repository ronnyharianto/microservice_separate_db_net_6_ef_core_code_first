namespace Falcon.BackEnd.Notifications.Controllers.Notifications.Inputs
{
    public class NotificationInput
    {
        public List<string> Target { get; set; } = new List<string>();
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;

    }
}
