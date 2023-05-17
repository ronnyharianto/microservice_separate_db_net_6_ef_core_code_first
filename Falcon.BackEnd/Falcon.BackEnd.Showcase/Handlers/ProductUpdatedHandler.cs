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
    public class ProductUpdatedResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Remark { get; set; }
    }

    public class ProductUpdatedHandler : ISubsriberHandler<ProductUpdated>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly HttpClientHelper _httpClient;

        public ProductUpdatedHandler(ApplicationDbContext dbContext, IMapper mapper, HttpClientHelper httpClient)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpClient = httpClient;
        }

        [CapSubscribe(nameof(ProductUpdated))]
        public object Handle(ProductUpdated message)
        {
            string url = String.Format("{0}{1}{2}", ServiceConstants.ProductService, "api/v1/product/view/", message.ProductId.ToString());
            var responseData = _httpClient.GetObjectResult<ProductUpdatedResponse>(url);

            if (responseData != null && responseData.Obj != null && responseData.Id != Guid.Empty)
            {
				var searchDataProduct = _dbContext.ProductViewModels.FirstOrDefault(x => x.Id == message.ProductId);

				if (searchDataProduct != null)
				{
                    searchDataProduct.Name = responseData.Obj.Name;
                    searchDataProduct.Remark = responseData.Obj.Remark;
				}

                return new { ProductId = responseData.Id, IsSuccess = true };
            }

            return new { ProductId = Guid.Empty, IsSuccess = false };
        }
    }
}