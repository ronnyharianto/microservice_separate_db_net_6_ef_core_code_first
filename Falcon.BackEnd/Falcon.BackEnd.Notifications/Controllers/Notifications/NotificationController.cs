using Falcon.BackEnd.Notifications.Controllers.Notifications.CustomModels;
using Falcon.BackEnd.Notifications.Controllers.Notifications.Inputs;
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
    }
}
