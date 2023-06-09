﻿using Falcon.Libraries.DataAccess.Domain;

namespace Falcon.BackEnd.Notifications.Domain.Models.Entities
{
    public class UserNotification : EntityBase
    {
        public int UserId { get; set; }
        public string FcmToken { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public virtual List<ReadNotification> ReadNotification { get; set; } = new();
    }
}