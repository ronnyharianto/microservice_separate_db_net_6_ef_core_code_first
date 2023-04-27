using DotNetCore.CAP;
using Falcon.Models.Topics;

namespace Falcon.BackEnd.Showcase.Handlers
{
    public interface ISubscribeHandler
    {
        void Handle(ProductCreated message);
    }

    public class ProductCreatedHandler : ISubscribeHandler, ICapSubscribe
    {
        [CapSubscribe(nameof(ProductCreated))]
        public void Handle(ProductCreated message)
        {
            Console.WriteLine(message.ProductCode);
        }
    }
}