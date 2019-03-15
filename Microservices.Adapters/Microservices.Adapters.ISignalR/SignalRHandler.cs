using Microservices.Base;

namespace Microservices.Adapters.ISignalR
{
    public abstract class SignalRHandler : ISignalRHandler
    {
        public abstract SignalResponse Execute(SignalRequest request);

        public IResponse Execute(IRequest request)
        {
            return Execute(request as SignalRequest) as IResponse;
        }
    }
}
