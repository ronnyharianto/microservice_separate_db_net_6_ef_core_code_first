using DotNetCore.CAP;

namespace Falcon.Libraries.Microservice.Subscriber
{
    public interface ISubsriberHandler<in T> : ICapSubscribe
    {
        object Handle(T message);
    }
}
