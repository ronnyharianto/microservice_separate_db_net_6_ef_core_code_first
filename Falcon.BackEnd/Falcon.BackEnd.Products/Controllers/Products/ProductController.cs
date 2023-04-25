using Falcon.BackEnd.Products.Controllers.Products.CustomModels;
using Falcon.BackEnd.Products.Controllers.Products.Inputs;
using Falcon.BackEnd.Products.Domain.Models.Entities;
using Falcon.BackEnd.Products.Service.Products;
using Falcon.Libraries.Common.Object;
using Falcon.Models.Topics;
using KafkaFlow.Producers;
using Microsoft.AspNetCore.Mvc;

namespace Falcon.BackEnd.Products.Controllers.Products
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        private readonly IProducerAccessor _producer;

        public ProductController(ProductService productService, IProducerAccessor producer)
        {
            _productService = productService;
            _producer = producer;
        }

        [HttpPost("create")]
        public ObjectResult<ProductDto> CreateProduct(ProductInput data)
        {
            var retVal = _productService.Create(data);

            if (retVal.Obj != null)
            {
                _producer.GetProducer("general-producer").ProduceAsync(nameof(ProductCreated), null, new ProductCreated
                {
                    ProductId = retVal.Obj.Id,
                    ProductCode = retVal.Obj.Code,
                    ProductName = retVal.Obj.Name,
                    VariantNames = retVal.Obj.ProductVariants.Select(x => x.VariantName).ToList()
                });
            }

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
