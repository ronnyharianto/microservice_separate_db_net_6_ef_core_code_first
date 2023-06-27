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
            CreateMap<NotifInput, NotifDto>();
            CreateMap<UserNotifInput, UserNotif>();
            CreateMap<UserNotif, UserNotifDto>();
            CreateMap<NotifTemplateInput, NotifTemplateDto>();
            CreateMap<NotifTemplateInput, NotificationTemplate>();
            CreateMap<NotificationTemplate, NotifTemplateDto>();

            CreateMap<NotifTemplateUpdate, NotificationTemplate>()
                .ForMember(e => e.Code, opt => opt.Ignore());
        }
    }
}
