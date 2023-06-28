using AutoMapper;
using Falcon.BackEnd.Notifications.Controllers.Notifications.CustomModels;
using Falcon.BackEnd.Notifications.Controllers.Notifications.Inputs;
using Falcon.BackEnd.Notifications.Domain.Models.Entities;

namespace Falcon.BackEnd.Notifications.Configuration
{
    public class AutoMapperConfigurations : Profile
    {
        public AutoMapperConfigurations()
        {
            CreateMap<NotificationInput, NotificationDto>();
            CreateMap<NotificationDto, Notification>();
            CreateMap<UserNotificationInput, UserNotification>();
            CreateMap<UserNotification, UserNotificationDto>();
            CreateMap<NotificationTemplateInput, NotificationTemplateDto>();
            CreateMap<NotificationTemplateInput, NotificationTemplate>();
            CreateMap<NotificationTemplate, NotificationTemplateDto>();

            CreateMap<NotificationTemplateUpdate, NotificationTemplate>()
                .ForMember(e => e.Code, opt => opt.Ignore());

            CreateMap<ReadNotificationInput, ReadNotification>();
            CreateMap<ReadNotification, ReadNotificationDto>();
        }
    }
}
