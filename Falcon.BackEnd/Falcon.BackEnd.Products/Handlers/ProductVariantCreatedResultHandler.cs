using AutoMapper;
using DotNetCore.CAP;
using Falcon.BackEnd.Products.Domain;
using Falcon.Libraries.Common.Helper;
using Falcon.Libraries.Microservice.Subscriber;
using Falcon.Models.Topics;
using System.Text.Json;

namespace Falcon.BackEnd.Products.Handlers
{
    public class ProductVariantCreatedResultHandler : ISubsriberHandler<JsonElement>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly HttpClientHelper _httpClient;

        public ProductVariantCreatedResultHandler(ApplicationDbContext dbContext, IMapper mapper, HttpClientHelper httpClient)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpClient = httpClient;
        }

        [CapSubscribe(nameof(ProductVariantCreated) + "Result")]
        public object Handle(JsonElement param)
        {
            var productVariantId = param.GetProperty("ProductVariantId").GetGuid();
            var isSuccess = param.GetProperty("IsSuccess").GetBoolean();

            Console.WriteLine($"ProductVariantId : {productVariantId} | IsSuccess : {isSuccess}");

            return default!;
        }
    }
}
