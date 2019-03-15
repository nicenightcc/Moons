using Microservices.Base;

namespace Microservices.Adapters.ISignalR
{
    public class SignalResponse : IResponse
    {
        public ProcessCode Code { get; set; }
        public string Message { get; set; }
    }
}
