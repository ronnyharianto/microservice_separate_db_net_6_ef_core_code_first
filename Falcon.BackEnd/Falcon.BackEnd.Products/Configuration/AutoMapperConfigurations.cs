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
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.ProductCode, o => o.MapFrom(s => s.Code))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.VariantNames, o => o.MapFrom(s => s.ProductVariants.Select(x => x.VariantName)));
        }
    }
}
