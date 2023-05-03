using AutoMapper;
using Falcon.BackEnd.Products.Controllers.Products.CustomModels;
using Falcon.BackEnd.Products.Controllers.Products.Inputs;
using Falcon.BackEnd.Products.Domain.Models.Entities;
using Falcon.Models.Topics;

namespace Falcon.BackEnd.Products.Configuration
{
    public class AutoMapperConfigurations : Profile
    {
        public AutoMapperConfigurations()
        {
            CreateMap<ProductInput, Product>();
            CreateMap<ProductVariantInput, ProductVariant>();
            CreateMap<Product, ProductDto>();
            CreateMap<ProductVariant, ProductVariantDto>();
            CreateMap<ProductDto, ProductCreated>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Id));
        }
    }
}
