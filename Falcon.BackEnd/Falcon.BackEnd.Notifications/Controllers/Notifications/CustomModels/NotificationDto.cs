using Falcon.Models.Enums;

namespace Falcon.BackEnd.Notifications.Controllers.Notifications.CustomModels
{
    public class NotificationDto
    {
        public string Target { get; set; } = string.Empty;
        public int? ReceiveUserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? NotificationCode { get; set; }
        public NotificationCategory Category { get; set; }
        public int TotalAudience { get; set; }
    }
}
