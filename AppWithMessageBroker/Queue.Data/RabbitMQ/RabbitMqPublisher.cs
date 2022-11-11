using Microsoft.Extensions.Options;
using Queue.Data.Abstract;
using Queue.Data.Data.Resources.Enum;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Queue.Data.RabbitMQ
{
    public class RabbitMqPublisher : IQueuePublisher
    {
        private readonly IQueueConnection _connectionService;
        public RabbitMqPublisher(IOptions<IRabbitMqConfig> config)
        {
            _connectionService = new QueueConnectionRepository(config);
        }


        public void Publish<T>(T eventBody, string queueName, string exchangeName, string routingKey, ExchType exchType)
            where T : class, IEventBody, new()
        {
            var channel = _connectionService.CreateChannel(exchangeName, queueName, exchType, routingKey);
            var bodyString = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(eventBody));
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            channel.BasicPublish(exchangeName, routingKey, properties, bodyString);
        }

        public void Publish<T>(T eventBody, string exchangeName, string routingKey, ExchType exchType)
            where T : class, IEventBody, new()
        {
            var channel = _connectionService.CreateChannel(exchangeName, exchType);
            var bodyString = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(eventBody));
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            channel.BasicPublish(exchangeName, routingKey, properties, bodyString);
        }

        public void Publish<T>(T eventBody, string queueName)
            where T : class, IEventBody, new()
        {
            var channel = _connectionService.CreateChannel(queueName);
            var bodyString = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(eventBody));
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            channel.BasicPublish(string.Empty, queueName, properties, bodyString);
        }

        public void Publish(Stream stream, string queueName, string exchangeName, string routingKey, ExchType exchType)
        {
            var channel = _connectionService.CreateChannel(exchangeName, queueName, exchType, routingKey);

            using MemoryStream ms = new();
            stream.CopyTo(ms);
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            channel.BasicPublish(exchangeName, routingKey, properties, ms.ToArray());
        }
    }
}
