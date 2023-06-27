using Falcon.Libraries.DataAccess.Domain;

namespace Falcon.BackEnd.Notifications.Domain.Models.Entities
{
    public class NotificationTemplate : EntityBase
    {
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public virtual List<Notification>? Notification { get; set; }
    }
}
