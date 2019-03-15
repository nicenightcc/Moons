using Microservices.Adapters.IMessageQueue;
using Microservices.Common;
using Microservices.IoC;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Microservices.Adapters.RabbitMQ
{
    public class RabbitMQAdapter : IMQAdapter
    {
        private RabbitMQProductor client;
        private Type[] mqHandlers;
        public string Name { get => "RabbitMQAdapter"; }
        public Type TargetType { get => typeof(IMQAdapter); }
        public RabbitMQAdapter()
        {
            var config = Config.Root["RabbitMQ"];
            if (string.IsNullOrEmpty(config))
                throw new Exception("Config Not Found: [RabbitMQ]");
            this.client = new RabbitMQProductor(config["Url"], config["Queue"], config["Exchange"]);

            this.mqHandlers = IoCFac.Instance.GetClassList<IMQHandler>()?
                .Distinct()?.ToArray();
        }

        public void Publish<TMQResquest>(TMQResquest request) where TMQResquest : IMQRequest
        {
            var mQhanderType = mqHandlers.FirstOrDefault(en => en.BaseType.GenericTypeArguments[0] == typeof(TMQResquest));
            if (mQhanderType == null)
                throw new Exception("MQHandler Not Found: " + typeof(TMQResquest).FullName);
            var name = mQhanderType.FullName;
            if (name.EndsWith("Handler"))
                name = name.Substring(0, name.Length - 7);
            var body = JsonConvert.SerializeObject(request);
            try
            {
                client.Publish(name, body);
            }
            catch (Exception e)
            {
               throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public void Publish<TMQResquest>(IMQRequest request) where TMQResquest : IMQRequest
        {
            Publish((TMQResquest)request);
        }
    }
}
