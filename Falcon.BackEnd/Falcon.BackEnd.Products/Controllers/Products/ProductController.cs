using AutoMapper;
using DotNetCore.CAP;
using Falcon.BackEnd.Products.Controllers.Products.CustomModels;
using Falcon.BackEnd.Products.Controllers.Products.Inputs;
using Falcon.BackEnd.Products.Domain.Models.Entities;
using Falcon.BackEnd.Products.Service.Products;
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
        private readonly ICapPublisher _publisher;
        private readonly IMapper _mapper;

        public ProductController(ProductService productService, ICapPublisher publisher, IMapper mapper)
        {
            _productService = productService;
            _publisher = publisher;
            _mapper = mapper;
        }

        [HttpPost("create")]
        public ObjectResult<ProductDto> CreateProduct(ProductInput data)
        {
            var retVal = _productService.Create(data);

            if (retVal.Succeeded && retVal.Obj != null)
            {
                _publisher.Publish(nameof(ProductCreated), _mapper.Map<ProductCreated>(retVal.Obj), nameof(ProductCreated) + "Result");
            }

            return retVal;
        }

		[HttpPost("deleteproduct")]
		public ServiceResult DeleteProduct(Guid Id)
		{
			var retVal = _productService.DeleteProduct(Id);

			if (retVal.Succeeded)
			{
				_publisher.Publish(nameof(ProductDeleted), retVal.Id, nameof(ProductDeleted) + "Result");
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
		public ServiceResult UpdateProduct(Guid Id, ProductUpdate productUpdate)
		{
			var retVal = _productService.UpdateProduct(Id, productUpdate);

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
    }
}
