using AutoMapper;
using Falcon.BackEnd.Notifications.Controllers.Notifications.CustomModels;
using Falcon.BackEnd.Notifications.Controllers.Notifications.Inputs;
using Falcon.BackEnd.Notifications.Domain;
using Falcon.BackEnd.Notifications.Domain.Models.Entities;
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

        #region Mutation
        public async Task<ObjectResult<NotificationDto>> CreateNotif(NotificationInput input)
        {
            int result = 0;
            var retVal = new ObjectResult<NotificationDto>(ServiceResultCode.BadRequest);

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
                retVal.Obj = _mapper.Map<NotificationDto>(input);
                retVal.OK("notif complete by Target(Topic or User) " + $"{result}" + "/" + $"{input.Target.Count}");
            }

            return retVal;
        }

        public ObjectResult<UserNotificationDto> CreateUserNotif(UserNotificationInput input)
        {
            var retVal = new ObjectResult<UserNotificationDto>(ServiceResultCode.BadRequest);

            var newData = _mapper.Map<UserNotificationInput, UserNotification>(input);

            if (newData != null)
            {
                _dbContext.UserNotif.Add(newData);

                retVal.Obj = _mapper.Map<UserNotification, UserNotificationDto>(newData);
                retVal.OK(null);
            }

            return retVal;
        }
        
        public ObjectResult<NotificationTemplateDto> CreateNotifTemplate(NotificationTemplateInput input)
        {
            var retVal = new ObjectResult<NotificationTemplateDto>(ServiceResultCode.BadRequest);

            var newData = _mapper.Map<NotificationTemplateInput, NotificationTemplate>(input);

            if (newData != null)
            {
                _dbContext.NotificationTemplate.Add(newData);

                retVal.Obj = _mapper.Map<NotificationTemplate, NotificationTemplateDto>(newData);
                retVal.OK(null);
            }

            return retVal;
        }
        
        public ServiceResult UpdateNotifTemplate(NotificationTemplateUpdate input)
        {
            var retVal = new ServiceResult(ServiceResultCode.NotFound);

            var searchDataNotifTemplate = _dbContext.NotificationTemplate.Where(e => e.Id == input.Id).FirstOrDefault();

            if (searchDataNotifTemplate != null)
            {
                _mapper.Map(input, searchDataNotifTemplate);

                _dbContext.NotificationTemplate.Update(searchDataNotifTemplate);

                retVal.OK(null);
            }

            return retVal;
        }

        public ObjectResult<ReadNotificationDto> CreateReadNotif(ReadNotificationInput input)
        {
            var retVal = new ObjectResult<ReadNotificationDto>(ServiceResultCode.BadRequest);

            var newData = _mapper.Map<ReadNotificationInput, ReadNotification>(input);

            if (newData != null)
            {
                _dbContext.ReadNotif.Add(newData);

                retVal.Obj = _mapper.Map<ReadNotification, ReadNotificationDto>(newData);
                retVal.OK(null);
            }

            return retVal;
        }

        #endregion

        #region Query
        public ObjectResult<IQueryable<NotificationTemplate>> GetListAllNotifTemplate()
        {
            var retVal = new ObjectResult<IQueryable<NotificationTemplate>>(ServiceResultCode.BadRequest)
            {
                Obj = _dbContext.NotificationTemplate.AsQueryable()
            };
            retVal.OK(null);

            return retVal;
        }
        #endregion
    }
}
