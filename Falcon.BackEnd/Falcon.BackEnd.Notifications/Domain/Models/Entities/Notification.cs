using Falcon.Libraries.DataAccess.Domain;
using Falcon.Models.Enums;

namespace Falcon.BackEnd.Notifications.Domain.Models.Entities
{
    public class Notification : EntityBase
    {
		public NotificationCategory Category { get; set; }
		public string Target { get; set; } = string.Empty;
        public int? ReceiveUserId { get; set; }
		public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
		public string? NotificationCode { get; set; }
		public string? TitleTemplate { get; set; }
        public string? ContentTemplate { get; set; }
        public int TotalAudience { get; set; }
        public virtual List<ReadNotification>? ReadNotification { get; set; }
    }
}