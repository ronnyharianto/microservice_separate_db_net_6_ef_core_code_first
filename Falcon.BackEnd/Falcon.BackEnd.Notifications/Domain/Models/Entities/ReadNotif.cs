using Falcon.Libraries.DataAccess.Domain;

namespace Falcon.BackEnd.Notifications.Domain.Models.Entities
{
    public class ReadNotif : EntityBase
    {
        public Guid IdNotif { get; set; }
        public Guid UserNotifId { get; set; }
        public virtual UserNotif? UserNotif { get; set; }
        public virtual Notification? Notification { get; set; }

    }
}
