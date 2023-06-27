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
        public async Task<ObjectResult<NotifDto>> CreateNotif(NotifInput data)
        {
            var retVal = await _notificationService.CreateNotif(data);

            return retVal;
        }
        
        [HttpPost("createusernotif")]
        public ObjectResult<UserNotifDto> CreateUserNotif(UserNotifInput data)
        {
            var retVal = _notificationService.CreateUserNotif(data);

            return retVal;
        }
        
        [HttpPost("createnotiftemplate")]
        public ObjectResult<NotifTemplateDto> CreateNotifTemplate(NotifTemplateInput data)
        {
            var retVal = _notificationService.CreateNotifTemplate(data);

            return retVal;
        }
        
        [HttpPost("updatenotiftemplate")]
        public ServiceResult UpdateNotifTemplate(NotifTemplateUpdate data)
        {
            var retVal = _notificationService.UpdateNotifTemplate(data);

            return retVal;
        }

        [HttpGet("viewlistallnotiftemplate")]
        public ObjectResult<IQueryable<NotificationTemplate>> GetListAllNotifTemplate()
        {
            return _notificationService.GetListAllNotifTemplate();
        }
    }
}
