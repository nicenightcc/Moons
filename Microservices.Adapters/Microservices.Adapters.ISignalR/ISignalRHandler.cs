using Microservices.Base;

namespace Microservices.Adapters.ISignalR
{
    public interface ISignalRHandler : IHandler
    {
        SignalResponse Execute(SignalRequest request);
    }
}
