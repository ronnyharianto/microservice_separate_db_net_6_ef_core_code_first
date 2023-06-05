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
    public class ProductVariantResponse
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string VariantName { get; set; } = string.Empty;
    }
    public class ProductVariantCreatedHandler : ISubsriberHandler<ProductVariantCreated>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly HttpClientHelper _httpClient;

        public ProductVariantCreatedHandler(ApplicationDbContext dbContext, IMapper mapper, HttpClientHelper httpClient)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpClient = httpClient;
        }

        [CapSubscribe(nameof(ProductVariantCreated))]
        public object Handle(ProductVariantCreated message)
        {
            string url = String.Format("{0}{1}{2}", ServiceConstants.ProductService, "api/v1/product/viewvariant/", message.ProductVariantId.ToString());
            var responseData = _httpClient.GetObjectResult<ProductVariantResponse>(url);
            var productId = _dbContext.ProductViewModels.Where(x => x.Id == responseData!.Obj!.ProductId).FirstOrDefault();

            if (responseData != null && responseData.Obj != null && productId != null)
            {
                var productVariantViewModel = _mapper.Map<ProductVariantViewModel>(responseData.Obj);
                _dbContext.ProductVariantViewModels.Add(productVariantViewModel);

                return new { ProductVariantId = productVariantViewModel.Id, IsSuccess = true };
            }

            return new { ProductVariantId = Guid.Empty, IsSuccess = false };
        }
    }
}
