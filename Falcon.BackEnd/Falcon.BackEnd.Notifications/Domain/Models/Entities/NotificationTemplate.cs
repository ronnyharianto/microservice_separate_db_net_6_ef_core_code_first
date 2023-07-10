using Falcon.Libraries.DataAccess.Domain;

namespace Falcon.BackEnd.Notifications.Domain.Models.Entities
{
    public class NotificationTemplate : EntityBase
    {
		public string Code { get; set; } = string.Empty;
		public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Description { get; set;} = string.Empty;
    }
}