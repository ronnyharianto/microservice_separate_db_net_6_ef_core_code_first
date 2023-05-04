using AutoMapper;
using Falcon.BackEnd.Products.Common;
using Falcon.BackEnd.Products.Controllers.Products.CustomModels;
using Falcon.BackEnd.Products.Controllers.Products.Inputs;
using Falcon.BackEnd.Products.Domain;
using Falcon.BackEnd.Products.Domain.Models.Entities;
using Falcon.Libraries.Common.Enums;
using Falcon.Libraries.Common.Helper;
using Falcon.Libraries.Common.Object;
using Falcon.Libraries.Microservice.Services;
using Microsoft.EntityFrameworkCore;

namespace Falcon.BackEnd.Products.Service.Products
{
    public class ProductService : BaseService<ApplicationDbContext>
    {
        private readonly CacheHelper _cacheHelper;
        public ProductService(ApplicationDbContext dbContext, IMapper mapper, CacheHelper cacheHelper) : base(dbContext, mapper)
        {
            _cacheHelper = cacheHelper;
        }

        public ObjectResult<ProductDto> Create(ProductInput data)
        {
            var retVal = new ObjectResult<ProductDto>(ServiceResultCode.BadRequest);
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
            var cacheData = _cacheHelper.GetCacheData<List<Product>>(ApplicationConstans.CacheKey.ProductData);
            var retVal = new ObjectResult<IQueryable<Product>>(ServiceResultCode.BadRequest);

            if (cacheData != null)
            {
                retVal.Obj = cacheData.AsQueryable();
            }
            else
            {
                retVal.Obj = _dbContext.Products.Include(x => x.ProductVariants).AsQueryable();
                _cacheHelper.SetCacheData<List<Product>>(ApplicationConstans.CacheKey.ProductData, retVal.Obj.ToList(), TimeSpan.FromMinutes(5));
            }
            retVal.OK(null);

            return retVal;
        }

        public ObjectResult<IQueryable<Product>> GetListAllProducts()
        {
            var retVal = new ObjectResult<IQueryable<Product>>(ServiceResultCode.BadRequest)
            {
                Obj = _dbContext.Products.Include(x => x.ProductVariants).IgnoreQueryFilters().AsQueryable()
            };
            retVal.OK(null);

            return retVal;
        }


        public ObjectResult<Product> GetDetailProduct(Guid id)
        {
            var retVal = new ObjectResult<Product>(ServiceResultCode.Ok)
            {
                Obj = _dbContext.Products.Include(x => x.ProductVariants).Where(x => x.Id == id).FirstOrDefault()
            };

            return retVal;
        }
    }
}
