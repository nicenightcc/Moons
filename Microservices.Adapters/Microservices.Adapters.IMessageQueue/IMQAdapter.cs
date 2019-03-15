using Microservices.Base;

namespace Microservices.Adapters.IMessageQueue
{
    public interface IMQAdapter : IAdapter
    {
        void Publish<TMQResquest>(IMQRequest request) where TMQResquest : IMQRequest;
        void Publish<TMQResquest>(TMQResquest request) where TMQResquest : IMQRequest;
    }
}
