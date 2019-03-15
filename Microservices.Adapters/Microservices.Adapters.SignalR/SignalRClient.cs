using Microservices.Adapters.ISignalR;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Adapters.SignalR
{
    public class SignalRClient : IDisposable
    {
        private HubConnection connection = null;
        private IHubProxy hub = null;
        private Dictionary<string, Type> handlers;

        public SignalRClient(string url)
        {
            this.connection = new HubConnection(url.Substring(0, url.LastIndexOf('/')));
            this.hub = connection.CreateHubProxy(url.Substring(url.LastIndexOf('/') + 1));
            this.handlers = IoC.IoCFac.Instance.GetClassList<ISignalRHandler>().ToDictionary(k =>
            {
                var name = k.Name.ToLower();
                return name.EndsWith("handler") ? name.Substring(0, name.Length - 7) : name;
            }, v => v);
            connection.Received += Received;
            connection.Start().Wait();
        }

        private void Received(string message)
        {
            Console.WriteLine(message);
            try
            {
                var request = JsonConvert.DeserializeObject<SignalRequest>(message);
                if (request != null)
                {
                    var name = request.Type.ToLower();
                    if (name.EndsWith("handler") || name.EndsWith("request"))
                        name = name.Substring(0, name.Length - 7);
                    if (handlers.ContainsKey(name))
                    {
                        var handler = Activator.CreateInstance(handlers[name]) as ISignalRHandler;
                        handler.Execute(request);
                    }
                }
            }
            catch { }
        }

        public void Send(SignalRequest request)
        {
            if (connection.State != ConnectionState.Connected)
                connection.Start().Wait();
            hub.Invoke("Send", request).Wait();
        }

        public async Task SendAsync(SignalRequest request)
        {
            if (connection.State != ConnectionState.Connected)
                await connection.Start();
            await hub.Invoke("Send", request);
        }

        public void Close()
        {
            if (connection != null && connection.State == ConnectionState.Connected)
                connection.Stop();
        }

        public void Dispose()
        {
            Close();
            connection.Dispose();
        }
    }
}
