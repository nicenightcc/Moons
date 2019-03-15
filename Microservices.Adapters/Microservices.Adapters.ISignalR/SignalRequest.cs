using Microservices.Base;
using System;

namespace Microservices.Adapters.ISignalR
{
    public class SignalRequest : IRequest
    {
        public string Sender { get; set; }
        public string[] Receiver { get; set; } = new string[] { };
        public string Content { get; set; }
        public string Type { get; set; }
        public string Time { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
