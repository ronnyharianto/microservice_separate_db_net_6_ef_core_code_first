namespace Falcon.BackEnd.Notifications.Controllers.Notifications.CustomModels
{
    public class UserNotifDto
    {
        public int UserId { get; set; }
        public string FcmToken { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}
