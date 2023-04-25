using AutoMapper;
using Falcon.BackEnd.Products.Controllers.Products.CustomModels;
using Falcon.BackEnd.Products.Controllers.Products.Inputs;
using Falcon.BackEnd.Products.Domain;
using Falcon.BackEnd.Products.Domain.Models.Entities;
using Falcon.Libraries.Common.Object;
using Falcon.Libraries.Microservice.Services;
using Microsoft.EntityFrameworkCore;

namespace Falcon.BackEnd.Products.Service.Products
{
    public class ProductService : BaseService<ApplicationDbContext>
    {
        public ProductService(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

        public ObjectResult<ProductDto> Create(ProductInput data)
        {
            var retVal = new ObjectResult<ProductDto>();
            var newData = _mapper.Map<ProductInput, Product>(data);

            if (newData != null)
            {
                _dbContext.Products.Add(newData);

                retVal.Obj = _mapper.Map<Product, ProductDto>(newData);
                retVal.OK(null);
            }

            return retVal;
        }

        public ObjectResult<IQueryable<Product>> GetListProducts()
        {
            var retVal = new ObjectResult<IQueryable<Product>>
            {
                Obj = _dbContext.Products.Include(x => x.ProductVariants).AsQueryable()
            };
            retVal.OK(null);

            return retVal;
        }

        public ObjectResult<Product> GetDetailProduct(Guid id)
        {
            var retVal = new ObjectResult<Product>
            {
                Obj = _dbContext.Products.Include(x => x.ProductVariants).Where(x => x.Id == id).FirstOrDefault()
            };
            retVal.OK(null);

            return retVal;
        }
    }
}
