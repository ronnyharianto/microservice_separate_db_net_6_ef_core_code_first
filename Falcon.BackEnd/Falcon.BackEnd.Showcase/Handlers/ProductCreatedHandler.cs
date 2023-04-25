using Falcon.Models;
using KafkaFlow;
using KafkaFlow.TypedHandler;

namespace Falcon.BackEnd.Showcase.Handlers
{
    public class ProductCreatedHandler : IMessageHandler<ProductCreated>
    {
        public Task Handle(IMessageContext context, ProductCreated message)
        {
            Console.WriteLine(
                "Partition: {0} | Offset: {1} | Message: {2}",
                context.ConsumerContext.Partition,
                context.ConsumerContext.Offset,
                message.ProductCode);

            return Task.CompletedTask;
        }
    }
}