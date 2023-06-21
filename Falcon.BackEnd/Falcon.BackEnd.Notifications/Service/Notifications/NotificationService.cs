using AutoMapper;
using Falcon.BackEnd.Notifications.Controllers.Notifications.CustomModels;
using Falcon.BackEnd.Notifications.Controllers.Notifications.Inputs;
using Falcon.BackEnd.Notifications.Domain;
using Falcon.Libraries.Common.Enums;
using Falcon.Libraries.Common.Helper;
using Falcon.Libraries.Common.Object;
using Falcon.Libraries.Microservice.Services;
using Microsoft.Extensions.Options;

namespace Falcon.BackEnd.Notifications.Service.Notifications
{
    public class NotificationService : BaseService<ApplicationDbContext>
    {
        private readonly FcmNotificationSetting _fcmNotificationSetting;
        private readonly FirebaseNotificationHelper _firebaseNotificationHelper;

        public NotificationService(ApplicationDbContext dbContext, IMapper mapper, IOptions<FcmNotificationSetting> settings, FirebaseNotificationHelper firebaseNotificationHelper) : base(dbContext, mapper)
        {
            _fcmNotificationSetting = settings.Value;
            _firebaseNotificationHelper = firebaseNotificationHelper;
        }

        public async Task<ObjectResult<NotifDto>> CreateNotif(NotifInput input)
        {
            int result = 0;
            var retVal = new ObjectResult<NotifDto>(ServiceResultCode.BadRequest);

            foreach (string target in input.Target)
            {
                var sendNotif = await _firebaseNotificationHelper.SendNotif(_fcmNotificationSetting.ServerKey, target, input.Body, input.Title);

                if (sendNotif.Succeeded == true)
                {
                    result += 1;
                }
            }

            if (result > 0)
            {
                retVal.Obj = _mapper.Map<NotifDto>(input);
                retVal.OK("notif complete by Target(Topic or User) " + $"{result}" + "/" + $"{input.Target.Count}");
            }

            return retVal;
        }

    }
}
