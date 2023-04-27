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
                _publisher.Publish(nameof(ProductCreated), _mapper.Map<ProductCreated>(retVal.Obj));
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
