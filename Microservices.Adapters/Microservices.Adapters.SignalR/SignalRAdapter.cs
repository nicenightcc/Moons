using Microservices.Adapters.ISignalR;
using Microservices.Common;
using System;
using System.Threading.Tasks;

namespace Microservices.Adapters.SignalR
{
    public class SignalRAdapter : ISignalRAdapter, IDisposable
    {
        SignalRClient client;

        public string Name { get => "SignalRAdapter"; }
        public Type TargetType { get => typeof(ISignalRAdapter); }
        public SignalRAdapter()
        {
            string connstr = Config.Root["SignalR"];
            if (string.IsNullOrEmpty(connstr))
                throw new Exception("Config Not Found: [SignalR]");
            this.client = new SignalRClient(connstr);
        }

        public void Send(SignalRequest request)
        {
            try
            {
                this.client.Send(request);
            }
            catch (Exception e)
            {
               throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public async Task SendAsync(SignalRequest request)
        {
            try
            {
                await this.client.SendAsync(request);
            }
            catch (Exception e)
            {
               throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public void Close()
        {
            try
            {
                this.client.Close();
            }
            catch (Exception e)
            {
               throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public void Dispose()
        {
            Close();
        }
    }
}
