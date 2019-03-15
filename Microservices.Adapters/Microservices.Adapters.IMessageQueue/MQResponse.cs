using Microservices.Base;

namespace Microservices.Adapters.IMessageQueue
{
    public class MQResponse : IResponse
    {
        public ProcessCode Code { get; set; }
        public string Message { get; set; }
    }
}
