using Falcon.Models.Enums;

namespace Falcon.BackEnd.Notifications.Controllers.Notifications.Inputs
{
	//Perlu diubah setelah requirement pengiriman notif lebih jelas
	public class NotificationInput
	{
		public NotificationCategory Category { get; set; }
        public bool IsSentToAll { get; set; } = false;
		public List<string> Target { get; set; } = new List<string>();
        public string? NotificationCode { get; set; }
		public string? Title { get; set; }
		public string Body { get; set; } = string.Empty;
	}
}