using AutoMapper;
using Falcon.BackEnd.APIGateway.Controllers.APIGateway.CustomModels;
using Falcon.BackEnd.APIGateway.Domain.Model.Entities;

namespace Falcon.BackEnd.Notifications.Configuration
{
    public class AutoMapperConfigurations : Profile
    {
        public AutoMapperConfigurations()
        {
            CreateMap<LoginDto, Login>();
        }
    }
}
