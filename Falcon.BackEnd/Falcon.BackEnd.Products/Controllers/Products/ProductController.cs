using AutoMapper;
using DotNetCore.CAP;
using Falcon.BackEnd.Products.Controllers.Products.CustomModels;
using Falcon.BackEnd.Products.Controllers.Products.Inputs;
using Falcon.BackEnd.Products.Domain.Models.Entities;
using Falcon.BackEnd.Products.Service.Products;
using Falcon.Libraries.Common.Helper;
using Falcon.Libraries.Common.Object;
using Falcon.Models.Topics;
using Microsoft.AspNetCore.Mvc;

namespace Falcon.BackEnd.Products.Controllers.Products
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        private readonly JsonHelper _jsonHelper;
        private readonly ICapPublisher _publisher;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ProductController(ProductService productService, JsonHelper jsonHelper, ICapPublisher publisher, IMapper mapper, ILogger<ProductController> logger)
        {
            _productService = productService;
            _jsonHelper = jsonHelper;
            _publisher = publisher;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("create")]
        public ObjectResult<ProductDto> CreateProduct(ProductInput data)
        {
            _logger.LogInformation("ProductController - CreateProduct - Started");

            var retVal = _productService.Create(data);

            if (retVal.Succeeded && retVal.Obj != null)
            {
                var productCreated = _mapper.Map<ProductCreated>(retVal.Obj);

                _logger
                    .LogInformation("ProductController - CreateProduct - Publish Topic ProductCreated ({productCreated})", _jsonHelper.SerializeObject(productCreated));
                _publisher.Publish(nameof(ProductCreated), productCreated, nameof(ProductCreated) + "Result");
            }

            _logger.LogInformation("ProductController - CreateProduct - End");

            return retVal;
        }

        [HttpPost("creatlistvariant")]
        public ObjectResult<List<VariantProductDto>> CreateListVariant(VariantProductInput data)
        {
            var retVal = _productService.CreateListVariant(data);

            if (retVal.Succeeded && retVal.Obj != null)
            {
                foreach (var item in retVal.Obj)
                {
                    _publisher.Publish(nameof(ProductVariantCreated), _mapper.Map<ProductVariantCreated>(item), nameof(ProductVariantCreated) + "Result");
                }
            }

            return retVal;

        }

        [HttpPost("deleteproduct")]
        public ServiceResult DeleteProduct(Guid Id)
        {
            var retVal = _productService.DeleteProduct(Id);

            ProductDeleted productDeleted = new ProductDeleted
            {
                ProductId = Id
            };

            if (retVal.Succeeded)
            {
                _publisher.Publish(nameof(ProductDeleted), productDeleted, nameof(ProductDeleted) + "Result");
            }

            return retVal;
        }

        [HttpPost("deleteproductvariant")]
        public ServiceResult DeleteProductVariant(Guid Id)
        {
            var retVal = _productService.DeleteProductVariant(Id);

            return retVal;
        }

        [HttpPost("updateproduct")]
        public ServiceResult UpdateProduct(ProductUpdate productUpdate)
        {
            var retVal = _productService.UpdateProduct(productUpdate);

            ProductUpdated productUpdated = new ProductUpdated
            {
                ProductId = productUpdate.Id
            };

            if (retVal.Succeeded)
            {
                _publisher.Publish(nameof(ProductUpdated), productUpdated, nameof(ProductUpdated) + "Result");
            }

            return retVal;
        }

        [HttpPost("updateproductvariant")]
        public ServiceResult UpdateProductVariant(Guid Id, ProductVariantUpdate productVariantUpdate)
        {
            var retVal = _productService.UpdateProductVariant(Id, productVariantUpdate);

            return retVal;
        }

        [HttpGet("viewlist")]
        public ObjectResult<IQueryable<Product>> LoadProductLists()
        {
            return _productService.GetListProducts();
        }

        [HttpGet("viewalllist")]
        public ObjectResult<IQueryable<Product>> LoadAllProductLists()
        {
            return _productService.GetListAllProducts();
        }

        [HttpGet("view/{id}")]
        public ObjectResult<Product> LoadProductDetail(Guid id)
        {
            return _productService.GetDetailProduct(id);
        }

        [HttpGet("viewvariant/{id}")]
        public ObjectResult<ProductVariant> LoadProductVariant(Guid id)
        {
            return _productService.GetProductVariant(id);
        }
    }
}
