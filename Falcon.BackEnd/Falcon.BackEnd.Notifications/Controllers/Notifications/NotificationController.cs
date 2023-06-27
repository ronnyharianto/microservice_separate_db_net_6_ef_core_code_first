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

        [HttpPost("createnotif")]
        public async Task<ObjectResult<NotificationDto>> CreateNotif(NotificationInput data)
        {
            var retVal = await _notificationService.CreateNotif(data);

            return retVal;
        }
        
        [HttpPost("createusernotif")]
        public ObjectResult<UserNotificationDto> CreateUserNotif(UserNotificationInput data)
        {
            var retVal = _notificationService.CreateUserNotif(data);

            return retVal;
        }
        
        [HttpPost("createnotiftemplate")]
        public ObjectResult<NotificationTemplateDto> CreateNotifTemplate(NotificationTemplateInput data)
        {
            var retVal = _notificationService.CreateNotifTemplate(data);

            return retVal;
        }
        
        [HttpPost("updatenotiftemplate")]
        public ServiceResult UpdateNotifTemplate(NotificationTemplateUpdate data)
        {
            var retVal = _notificationService.UpdateNotifTemplate(data);

            return retVal;
        }

        [HttpGet("viewlistallnotiftemplate")]
        public ObjectResult<IQueryable<NotificationTemplate>> GetListAllNotifTemplate()
        {
            return _notificationService.GetListAllNotifTemplate();
        }

        [HttpPost("createreadnotif")]
        public ObjectResult<ReadNotificationDto> CreateReadNotif(ReadNotificationInput data)
        {
            var retVal = _notificationService.CreateReadNotif(data);

            return retVal;
        }
    }
}
