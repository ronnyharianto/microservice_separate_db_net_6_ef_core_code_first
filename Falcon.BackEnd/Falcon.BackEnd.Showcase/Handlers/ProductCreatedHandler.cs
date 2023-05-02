using AutoMapper;
using DotNetCore.CAP;
using Falcon.BackEnd.Showcase.Domain;
using Falcon.BackEnd.Showcases.Domain.Models.ViewModels;
using Falcon.Libraries.Common.Constants;
using Falcon.Libraries.Common.Object;
using Falcon.Libraries.Microservice.Subscriber;
using Falcon.Models.Topics;
using Newtonsoft.Json;

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

        public ProductCreatedHandler(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }   

        [CapSubscribe(nameof(ProductCreated))]
        public async void Handle(ProductCreated message)
        {
            HttpClient httpClient = new();

            Uri requestUri = new(InternalPortConstant.Products + "api/v1/product/view/" + message.ProductId.ToString());
            HttpRequestMessage requestMessage = new(HttpMethod.Get, requestUri);

            HttpResponseMessage responseMessage = httpClient.Send(requestMessage);
            responseMessage.EnsureSuccessStatusCode();
            string responseBody = await responseMessage.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<ObjectResult<ProductResponse>>(responseBody);

            if (responseData != null && responseData.Obj != null)
            {
                var productViewModel = _mapper.Map<ProductViewModel>(responseData.Obj);
                _dbContext.ProductViewModels.Add(productViewModel);
                _dbContext.SaveChanges();
            }
        }
    }
}