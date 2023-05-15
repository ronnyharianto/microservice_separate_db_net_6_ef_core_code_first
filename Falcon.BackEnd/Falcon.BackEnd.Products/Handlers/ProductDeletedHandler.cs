using AutoMapper;
using DotNetCore.CAP;
using Falcon.BackEnd.Products.Domain;
using Falcon.Libraries.Common.Constants;
using Falcon.Libraries.Common.Helper;
using Falcon.Libraries.Microservice.Subscriber;
using Falcon.Models.Topics;
using System.Text.Json;

namespace Falcon.BackEnd.Products.Handlers
{
    public class ProductDeletedResponse
    {
        public Guid Id { get; set; }
        //public string Code { get; set; } = string.Empty;
        //public string Name { get; set; } = string.Empty;
        //public string? Remark { get; set; }
    }

    public class ProductDeletedHandler : ISubsriberHandler<JsonElement>
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

        [CapSubscribe(nameof(ProductDeleted) + "Result")]
        public object Handle(JsonElement param)
        {
            var Id = param.GetProperty("Id").GetGuid();
            var isSuccess = param.GetProperty("IsSuccess").GetBoolean();

            Console.WriteLine($"Id : {Id} | IsSuccess : {isSuccess}");

            return default!;
        }
    }
}