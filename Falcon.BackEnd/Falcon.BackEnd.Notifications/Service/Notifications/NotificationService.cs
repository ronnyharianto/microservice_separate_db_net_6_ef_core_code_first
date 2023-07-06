using AutoMapper;
using Falcon.BackEnd.Notifications.Configuration;
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

        public async Task<ObjectResult<NotificationDto>> CreateNotification(NotificationInput input)
        {
            int result = 0;
            var retVal = new ObjectResult<NotificationDto>(ServiceResultCode.BadRequest);

            var userNotifications = _dbContext.UserNotification.Where(e => input.Target.Contains(e.UserName)).ToList();
            List<string> fcmToken = userNotifications.Select(e => e.FcmToken).ToList();
            var notificationTemplate= _dbContext.NotificationTemplate.Where(e => e.Code == input.NotificationCode).FirstOrDefault();

            List<NotificationDto> resultList = new();

            if (input.IsSentToAll == false)
            {
                foreach (var data in userNotifications)
                {
                    resultList.Add(new NotificationDto
                    {
                        Category = input.Category,
                        Target = data.FcmToken,
                        ReceiveUserId = data.UserId,
                        Title = notificationTemplate == null ? string.Empty : notificationTemplate.Title,
                        Body = notificationTemplate == null ? string.Empty : $"{input.ProgramName} {notificationTemplate.Content} {input.ToPosition}", // Perlu dilihat ulang dengan requirementnya
                        NotificationCode = notificationTemplate == null ? string.Empty : notificationTemplate.Code,
                        TitleTemplate = notificationTemplate == null ? string.Empty : notificationTemplate.Title,
                        ContentTemplate = notificationTemplate == null ? string.Empty : notificationTemplate.Content,
                        TotalAudience = 1
                    });
                }
            }
            else
            {
                fcmToken.Add(ApplicationConstants.TopicSentToAll);
                resultList.Add(new NotificationDto 
                {
                    Target = ApplicationConstants.TopicSentToAll,
                    Title = input.Title ?? string.Empty,
                    Body = input.Body,
                    Category = input.Category,
                    TotalAudience = _dbContext.UserNotification.Count()
                });
            }

            /*
             * 1. Simpan di database
             * 2. Kirim banyak notifikasi dalam 1 kali request ke Firebase
             * 3. Jika notif gagal dikirim, maka gagal kan operasi ke database juga (throw error)
             * */
            
            var newData = _mapper.Map<List<Notification>>(resultList);

            _dbContext.Notification.AddRange(newData);

            var sendNotif = await _firebaseNotificationHelper.SendNotif(_fcmNotificationSetting.ServerKey, fcmToken, newData.First().Body, newData.First().Title, input.IsSentToAll);

            if (sendNotif.Succeeded == true)
            {
                result += 1;
            }
            else
            {
                throw new Exception("Gagal Kirim Notif");
            }

            if (result > 0)
            {
                retVal.OK("notif complete");
            }

            return retVal;
        }

        public ObjectResult<UserNotificationDto> CreateUserNotification(UserNotificationInput input)
        {
            var retVal = new ObjectResult<UserNotificationDto>(ServiceResultCode.BadRequest);

            var newData = _mapper.Map<UserNotificationInput, UserNotification>(input);

            if (newData != null)
            {
                _dbContext.UserNotification.Add(newData);

                retVal.Obj = _mapper.Map<UserNotification, UserNotificationDto>(newData);
                retVal.OK("User Notification Has Created");
            }

            return retVal;
        }
        
        public ObjectResult<NotificationTemplateDto> CreateNotificationTemplate(NotificationTemplateInput input)
        {
            var retVal = new ObjectResult<NotificationTemplateDto>(ServiceResultCode.BadRequest);

            var newData = _mapper.Map<NotificationTemplateInput, NotificationTemplate>(input);

            if (newData != null)
            {
                _dbContext.NotificationTemplate.Add(newData);

                retVal.Obj = _mapper.Map<NotificationTemplate, NotificationTemplateDto>(newData);
                retVal.OK("Notification Template Has Created");
            }

            return retVal;
        }
        
        public ServiceResult UpdateNotificationTemplate(NotificationTemplateUpdate input)
        {
            var retVal = new ServiceResult(ServiceResultCode.NotFound);

            var searchDataNotifTemplate = _dbContext.NotificationTemplate.Where(e => e.Id == input.Id).FirstOrDefault();

            if (searchDataNotifTemplate != null)
            {
                _mapper.Map(input, searchDataNotifTemplate);

                _dbContext.NotificationTemplate.Update(searchDataNotifTemplate);

                retVal.OK("Update Notification Success");
            }

            return retVal;
        }

        public ObjectResult<ReadNotificationDto> CreateReadNotification(ReadNotificationInput input)
        {
            var retVal = new ObjectResult<ReadNotificationDto>(ServiceResultCode.BadRequest);

            var newData = _mapper.Map<ReadNotificationInput, ReadNotification>(input);

            if (newData != null)
            {
                _dbContext.ReadNotification.Add(newData);

                retVal.Obj = _mapper.Map<ReadNotification, ReadNotificationDto>(newData);
                retVal.OK("Read Notification Has Created");
            }

            return retVal;
        }

        #region Query
        public ObjectResult<IQueryable<NotificationTemplate>> GetListAllNotificationTemplate()
        {
            var retVal = new ObjectResult<IQueryable<NotificationTemplate>>(ServiceResultCode.BadRequest)
            {
                Obj = _dbContext.NotificationTemplate.AsQueryable()
            };
            retVal.OK("OK");

            return retVal;
        }
        #endregion
    }
}
