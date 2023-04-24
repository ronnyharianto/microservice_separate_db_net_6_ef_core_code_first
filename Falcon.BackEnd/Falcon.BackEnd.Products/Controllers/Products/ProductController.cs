using Falcon.BackEnd.Products.Controllers.Products.Inputs;
using Falcon.BackEnd.Products.Controllers.Products.Validators;
using Falcon.BackEnd.Products.Domain.Models.Entities;
using Falcon.BackEnd.Products.Service.Products;
using Falcon.Libraries.Common.Object;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Falcon.BackEnd.Products.Controllers.Products
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("create")]
        public ObjectResult<Product> CreateProduct(ProductInput data)
        {
            var retVal = _productService.Create(data);

            return retVal;
        }

        [HttpGet("viewlist")]
        public ObjectResult<IQueryable<Product>> LoadProductLists()
        {
            return _productService.GetListProducts();
        }

        [HttpGet("view/{id}")]
        public ObjectResult<Product> LoadProductDetail(Guid id)
        {
            return _productService.GetDetailProduct(id);
        }
    }
}
