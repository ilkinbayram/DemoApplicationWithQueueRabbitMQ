using Microsoft.Extensions.Options;
using Queue.Data.Abstract;
using Queue.Data.Data.Resources.Enum;
using Queue.Data.Data.Utitlites.Extensions;
using RabbitMQ.Client;

namespace Queue.Data.RabbitMQ
{
    public class QueueConnectionRepository : IQueueConnection, IDisposable
    {
        private readonly string _connectionString;
        private readonly IConnectionFactory _factory;
        private IModel _channel;
        private IConnection _connection;
        public QueueConnectionRepository(IOptions<IRabbitMqConfig> config)
        {
            _connectionString = config.Value.RMQ_CONNECTION_STRING;
            _factory = new ConnectionFactory() { DispatchConsumersAsync = true };
        }

        public IModel CreateChannel(string queueName)
        {
            SetChannelWithQueue(queueName);
            return _channel;
        }

        public IModel CreateChannel()
        {
            GenerateConnection();
            return _channel;
        }

        public IModel CreateChannel(string exchangeName, string queueName, ExchType exchangeType, string routingKey)
        {
            GenerateConnection();
            _channel = _channel.DeclareExchange(exchangeName, exchangeType)
                .DeclareQueue(queueName)
                .Bind(queueName, exchangeName, routingKey);
            return _channel;
        }

        public IModel CreateChannel(string exchangeName, ExchType exchangeType)
        {
            SetChannelWithExchange(exchangeName, exchangeType);
            return _channel;
        }

        private void SetChannelWithQueue(string queueName)
        {
            GenerateConnection();
            _channel = _channel.DeclareQueue(queueName);
        }

        private void SetChannelWithExchange(string exchangeName, ExchType exchType)
        {
            GenerateConnection();
            _channel = _channel.DeclareExchange(exchangeName, exchType);
        }

        private void GenerateConnection()
        {
            _factory.Uri = new Uri(_connectionString);
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void Dispose()
        {
            _channel?.Dispose();

            _connection?.Close();
            _connection?.Dispose();
        }
    }
}
