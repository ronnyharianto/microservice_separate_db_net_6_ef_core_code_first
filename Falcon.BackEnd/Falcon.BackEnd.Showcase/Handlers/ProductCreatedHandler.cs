using AutoMapper;
using DotNetCore.CAP;
using Falcon.BackEnd.Showcase.Domain;
using Falcon.BackEnd.Showcases.Domain.Models.ViewModels;
using Falcon.Libraries.Common.Constants;
using Falcon.Libraries.Common.Helper;
using Falcon.Libraries.Microservice.Subscriber;
using Falcon.Models.Topics;

namespace Falcon.BackEnd.Showcases.Handlers
{
    public class ProductResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Remark { get; set; }
    }

    public class ProductCreatedHandler : ISubsriberHandler<ProductCreated>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly HttpClientHelper _httpClient;

        public ProductCreatedHandler(ApplicationDbContext dbContext, IMapper mapper, HttpClientHelper httpClient)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpClient = httpClient;
        }

        [CapSubscribe(nameof(ProductCreated))]
        public void Handle(ProductCreated message)
        {
            string url = String.Format("{0}{1}{2}", ServiceConstants.ProductService, "api/v1/product/view/", message.ProductId.ToString());
            var responseData = _httpClient.GetObjectResult<ProductResponse>(url);

            if (responseData != null && responseData.Obj != null)
            {
                var productViewModel = _mapper.Map<ProductViewModel>(responseData.Obj);
                _dbContext.ProductViewModels.Add(productViewModel);
            }
        }
    }
}