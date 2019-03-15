using Microservices.Common;
using RabbitMQ.Client;
using System;
using System.Text;

namespace Microservices.Adapters.RabbitMQ
{
    public class RabbitMQProductor
    {
        private ConnectionFactory factory;
        private IConnection connection;
        private string queue;
        private string exchange;

        public RabbitMQProductor(string url, string queue, string exchange)
        {
            this.queue = queue;
            this.exchange = exchange;
            this.factory = new ConnectionFactory();
            this.factory.Uri = new Uri(url);
            this.connection = this.factory.CreateConnection();
        }

        public void Publish(string name, string body)
        {
            try
            {
                using (var channel = this.connection.CreateModel())
                {
                    var routingKey = name;
                    channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Direct, durable: true, autoDelete: false);
                    channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false);
                    channel.QueueBind(queue, exchange, ExchangeType.Direct, null);

                    var message = Encoding.UTF8.GetBytes(body);
                    channel.BasicPublish(exchange, routingKey, null, message);
                }
            }
            catch (Exception e)
            {
                throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }
    }
}
