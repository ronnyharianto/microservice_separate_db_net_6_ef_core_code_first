using Falcon.BackEnd.Notifications.Controllers.Notifications.CustomModels;
using Falcon.BackEnd.Notifications.Controllers.Notifications.Inputs;
using Falcon.BackEnd.Notifications.Domain.Models.Entities;
using Falcon.BackEnd.Notifications.Service.Notifications;
using Falcon.Libraries.Common.Object;
using Microsoft.AspNetCore.Mvc;

namespace Falcon.BackEnd.Notifications.Controllers.Notifications
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class NotificationController : Controller
    {
        private readonly NotificationService _notificationService;

        public NotificationController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost("createnotification")]
        public async Task<ObjectResult<NotificationDto>> CreateNotification(NotificationInput data)
        {
            var retVal = await _notificationService.CreateNotification(data);

            return retVal;
        }
        
        [HttpPost("createusernotification")]
        public ObjectResult<UserNotificationDto> CreateUserNotification(UserNotificationInput data)
        {
            var retVal = _notificationService.CreateUserNotification(data);

            return retVal;
        }
        
        [HttpPost("createnotificationtemplate")]
        public ObjectResult<NotificationTemplateDto> CreateNotificationTemplate(NotificationTemplateInput data)
        {
            var retVal = _notificationService.CreateNotificationTemplate(data);

            return retVal;
        }
        
        [HttpPost("updatenotificationtemplate")]
        public ServiceResult UpdateNotificationTemplate(NotificationTemplateUpdate data)
        {
            var retVal = _notificationService.UpdateNotificationTemplate(data);

            return retVal;
        }

        [HttpGet("viewlistallnotificationtemplate")]
        public ObjectResult<IQueryable<NotificationTemplate>> GetListAllNotificationTemplate()
        {
            return _notificationService.GetListAllNotificationTemplate();
        }

        [HttpPost("createreadnotification")]
        public ObjectResult<ReadNotificationDto> CreateReadNotification(ReadNotificationInput data)
        {
            var retVal = _notificationService.CreateReadNotification(data);

            return retVal;
        }
    }
}
