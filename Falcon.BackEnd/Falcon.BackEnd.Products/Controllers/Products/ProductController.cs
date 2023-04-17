using Falcon.BackEnd.Products.Controllers.Products.Inputs;
using Falcon.BackEnd.Products.Controllers.Products.Validators;
using Falcon.BackEnd.Products.Domain;
using Falcon.BackEnd.Products.Domain.Models.Entities;
using Falcon.Libraries.Common.Object;
using Falcon.Libraries.Common.Constants;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Falcon.BackEnd.Products.Service.Products;

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
            var retVal = new ObjectResult<Product>();

            ProductInputValidator validator = new ProductInputValidator();
            ValidationResult results = validator.Validate(data);

            if (results.IsValid)
            {
                retVal = _productService.Create(data);
            }

            return retVal;
        }

        [HttpGet("viewlist")]
        public ObjectResult<IQueryable<Product>> GetListProducts() 
        {
            return _productService.GetListProducts();
        }

        [HttpGet("view/{id}")]
        public ObjectResult<Product> GetDetailProduct(Guid id)
        {
            return _productService.GetDetailProduct(id);
        }
    }
}
