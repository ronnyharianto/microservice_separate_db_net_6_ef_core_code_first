using AutoMapper;
using DotNetCore.CAP;
using Falcon.BackEnd.Products.Controllers.Products.Inputs;
using Falcon.BackEnd.Showcase.Domain;
using Falcon.BackEnd.Showcases.Domain.Models.ViewModels;
using Falcon.Libraries.Common.Constants;
using Falcon.Libraries.Common.Helper;
using Falcon.Libraries.Microservice.Subscriber;
using Falcon.Models.Topics;

namespace Falcon.BackEnd.Showcases.Handlers
{
    public class ProductDeletedResponse
    {
        public Guid Id { get; set; }
        //public string Code { get; set; } = string.Empty;
        //public string Name { get; set; } = string.Empty;
        //public string? Remark { get; set; }
    }

    public class ProductDeletedHandler : ISubsriberHandler<ProductDeleted>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly HttpClientHelper _httpClient;

        public ProductDeletedHandler(ApplicationDbContext dbContext, IMapper mapper, HttpClientHelper httpClient)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpClient = httpClient;
        }

        [CapSubscribe(nameof(ProductDeleted))]
        public object Handle(ProductDeleted message)
        {
            string url = String.Format("{0}{1}{2}", ServiceConstants.ProductService, "api/v1/product/view/", message.ProductId.ToString());
            var responseData = _httpClient.GetObjectResult<ProductDeletedResponse>(url);

            if (responseData != null && responseData.Obj != null)
            {
				//var productViewModel = _mapper.Map<ProductViewModel>(responseData.Obj);
				var searchDataProduct = _dbContext.ProductViewModels.FirstOrDefault(x => x.Id == message.ProductId);
				//_dbContext.ProductViewModels.Update(productViewModel);

				if (searchDataProduct != null)
				{

                    //_mapper.Map(searchDataProduct.RowStatus = 1, searchDataProduct);
                    //_dbContext.ProductViewModels.Update(searchDataProduct);

                    searchDataProduct.RowStatus = 1;

				}

				//return new { ProductId = productViewModel.Id, IsSuccess = true };
            }

            return new { ProductId = Guid.Empty, IsSuccess = false };
        }
    }
}