using Microservices.Base;
using System.Threading.Tasks;

namespace Microservices.Adapters.ISignalR
{
    public interface ISignalRAdapter : IAdapter
    {
        void Send(SignalRequest model);
        Task SendAsync(SignalRequest model);
    }
}
