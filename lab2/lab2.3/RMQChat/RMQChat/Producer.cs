using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace RMQChat
{
    class Producer
    {
        public void Produce(Message message)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "chat", type: "fanout");

                var newMessage = JsonConvert.SerializeObject(message, Formatting.Indented);
                var body = Encoding.UTF8.GetBytes(newMessage);
                channel.BasicPublish(exchange: "chat",
                    routingKey: "",
                    basicProperties: null,
                    body: body);
            }
        }
    }
}
