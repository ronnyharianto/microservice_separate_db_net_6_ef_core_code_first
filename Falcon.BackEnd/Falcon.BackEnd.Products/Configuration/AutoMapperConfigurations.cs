using AutoMapper;
using Falcon.BackEnd.Products.Controllers.Products.CustomModels;
using Falcon.BackEnd.Products.Controllers.Products.Inputs;
using Falcon.BackEnd.Products.Domain.Models.Entities;

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
        }
    }
}
