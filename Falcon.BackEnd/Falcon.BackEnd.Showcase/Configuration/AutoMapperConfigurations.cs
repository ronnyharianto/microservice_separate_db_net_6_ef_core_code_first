using AutoMapper;
using Falcon.BackEnd.Showcases.Domain.Models.ViewModels;
using Falcon.BackEnd.Showcases.Handlers;

namespace Falcon.BackEnd.Showcases.Configuration
{
    public class AutoMapperConfigurations : Profile
    {
        public AutoMapperConfigurations()
        {
            CreateMap<ProductResponse, ProductViewModel>();
        }
    }
}
