using Falcon.Libraries.DataAccess.Domain;

namespace Falcon.BackEnd.Notifications.Domain.Models.Entities
{
    public class UserNotif : EntityBase
    {
        public int UserId { get; set; }
        public string FcmToken { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public virtual List<ReadNotif> ReadNotif { get; set; } = new();

    }
}
