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
        public ProductService(ApplicationDbContext dbContext) : base(dbContext) { }

        public ObjectResult<Product> Create(ProductInput data)
        {
            var retVal = new ObjectResult<Product>
            {
                Obj = new Product()
                {
                    Code = data.Code,
                    Name = data.Name,
                    Remark = data.Remark,
                    Price = data.Price,
                    ProductValidTo = DateTime.MaxValue
                }
            };

            _dbContext.Products.Add(retVal.Obj);

            retVal.OK(null);

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
