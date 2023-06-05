using AutoMapper;
using DotNetCore.CAP;
using Falcon.BackEnd.Products.Domain;
using Falcon.Libraries.Common.Helper;
using Falcon.Libraries.Microservice.Subscriber;
using Falcon.Models.Topics;
using System.Text.Json;

namespace Falcon.BackEnd.Products.Handlers
{
    public class ProductUpdatedResponse
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Remark { get; set; }
    }

    public class ProductUpdatedResultHandler : ISubsriberHandler<JsonElement>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly HttpClientHelper _httpClient;

        public ProductUpdatedResultHandler(ApplicationDbContext dbContext, IMapper mapper, HttpClientHelper httpClient)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpClient = httpClient;
        }

        [CapSubscribe(nameof(ProductUpdated) + "Result")]
        public object Handle(JsonElement param)
        {
            var productId = param.GetProperty("ProductId").GetGuid();
            var isSuccess = param.GetProperty("IsSuccess").GetBoolean();

            Console.WriteLine($"ProductId : {productId} | IsSuccess : {isSuccess}");

            return default!;
        }
    }
}