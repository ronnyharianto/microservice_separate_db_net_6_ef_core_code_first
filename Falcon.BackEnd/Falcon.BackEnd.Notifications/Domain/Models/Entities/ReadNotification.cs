using Falcon.Libraries.DataAccess.Domain;

namespace Falcon.BackEnd.Notifications.Domain.Models.Entities
{
    public class ReadNotification : EntityBase
    {
        public Guid NotificationId { get; set; }
        public Guid UserNotificationId { get; set; }
        public virtual UserNotification? UserNotification { get; set; }
        public virtual Notification? Notification { get; set; }
    }
}