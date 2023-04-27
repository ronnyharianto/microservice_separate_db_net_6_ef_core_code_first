using DotNetCore.CAP;
using Falcon.Libraries.Microservice.Subscriber;
using Falcon.Models.Topics;

namespace Falcon.BackEnd.Showcases.Handlers
{
    public class ProductCreatedHandler : ISubsriberHandler<ProductCreated>
    {
        [CapSubscribe(nameof(ProductCreated))]
        public void Handle(ProductCreated message)
        {
            Console.WriteLine(message.ProductCode);
        }
    }
}