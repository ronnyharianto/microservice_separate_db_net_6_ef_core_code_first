using Falcon.Libraries.DataAccess.Domain;

namespace Falcon.BackEnd.Notifications.Domain.Models.Entities
{
    public class Notification : EntityBase
    {
        public string Target { get; set; } = string.Empty;
        public string? ReceiveUserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? NotificationCode { get; set; }
        public string Category { get; set; } = string.Empty;
        public int TotalAudiance { get; set; } = 0 ;
        public virtual List<ReadNotif>? ReadNotif { get; set; }
        
    }
}
