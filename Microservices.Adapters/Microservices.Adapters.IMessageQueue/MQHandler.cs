using Microservices.Base;

namespace Microservices.Adapters.IMessageQueue
{
    public abstract class MQHandler<TRequest> : IMQHandler where TRequest : IMQRequest
    {
        public abstract MQResponse Execute(TRequest request);

        public IResponse Execute(IRequest request)
        {
            return Execute((TRequest)request);
        }
    }
}
