using AutoMapper;
using DotNetCore.CAP;
using Falcon.BackEnd.Products.Domain;
using Falcon.Libraries.Common.Helper;
using Falcon.Libraries.Microservice.Subscriber;
using Falcon.Models.Topics;
using System.Text.Json;

namespace Falcon.BackEnd.Products.Handlers
{
    public class ProductResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Remark { get; set; }
    }

    public class ProductCreatedResultHandler : ISubsriberHandler<JsonElement>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly HttpClientHelper _httpClient;

        public ProductCreatedResultHandler(ApplicationDbContext dbContext, IMapper mapper, HttpClientHelper httpClient)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpClient = httpClient;
        }

        [CapSubscribe(nameof(ProductCreated) + "Result")]
        public object Handle(JsonElement param)
        {
            var productId = param.GetProperty("ProductId").GetGuid();
            var isSuccess = param.GetProperty("IsSuccess").GetBoolean();

            Console.WriteLine($"ProductId : {productId} | IsSuccess : {isSuccess}");

            return default!;
        }
    }
}