using AutoMapper;
using Falcon.BackEnd.Products.Common;
using Falcon.BackEnd.Products.Controllers.Products.CustomModels;
using Falcon.BackEnd.Products.Controllers.Products.Inputs;
using Falcon.BackEnd.Products.Controllers.Products.Update;
using Falcon.BackEnd.Products.Domain;
using Falcon.BackEnd.Products.Domain.Models.Entities;
using Falcon.Libraries.Common.Enums;
using Falcon.Libraries.Common.Helper;
using Falcon.Libraries.Common.Object;
using Falcon.Libraries.Microservice.Services;
using Microsoft.AspNetCore.Mvc;
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

		public ServiceResult DeleteProduct(Guid Id)
		{
			var retval = new ServiceResult(ServiceResultCode.Error);

			var deleteDataProduct = _dbContext.Products.FirstOrDefault(x => x.Id == Id);
			var deleteDataProductVariant = _dbContext.ProductVariants.Where(x => x.ProductId == Id).ToList();

			if (deleteDataProduct != null)
			{
				deleteDataProduct.RowStatus = 1;

				if (deleteDataProductVariant != null)
				{
					foreach (var det in deleteDataProductVariant)
					{
						det.RowStatus = 1;
					}
				}

				retval.OK(null);
			}
			return retval;
		}

		public ServiceResult DeleteProductVariant(Guid Id)
		{
			var retval = new ServiceResult(ServiceResultCode.Error);

			var deleteDataProductVariant = _dbContext.ProductVariants.FirstOrDefault(x => x.Id == Id);

			if (deleteDataProductVariant != null)
			{
				deleteDataProductVariant.RowStatus = 1;

				retval.OK(null);
			}
			return retval;
		}

		public ServiceResult UpdateProduct(Guid Id, ProductUpdate productUpdate)
		{
			var retval = new ServiceResult(ServiceResultCode.NotFound);

			var searchDataProduct = _dbContext.Products.FirstOrDefault(x => x.Id == Id);

			if (searchDataProduct != null)
			{
				//_mapper.Map<ProductUpdate, Product>(productUpdate, searchDataProduct);

				_mapper.Map(productUpdate, searchDataProduct);

				//updateDataProduct.Code = productUpdate.Code;
				//updateDataProduct.Name = productUpdate.Name;
				//updateDataProduct.Remark = productUpdate.Remark;
				//updateDataProduct.Price = productUpdate.Price;

				//var UpdateData = _mapper.Map<Product, ProductDto>(UpdateDataProduct);

				_dbContext.Products.Update(searchDataProduct);

				retval.OK(null);
			}
			return retval;
		}

		public ServiceResult UpdateProductVariant(Guid Id, ProductVariantUpdate productVariantUpdate)
		{
			var retval = new ServiceResult(ServiceResultCode.NotFound);

			var updateDataProductVariant = _dbContext.ProductVariants.FirstOrDefault(x => x.Id == Id);

			if (updateDataProductVariant != null)
			{
				updateDataProductVariant.VariantName = productVariantUpdate.VariantName;

				retval.OK(null);
			}
			return retval;
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
                _cacheHelper.SetCacheData(ApplicationConstans.CacheKey.ProductData, retVal.Obj.ToList(), TimeSpan.FromMinutes(5));
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
