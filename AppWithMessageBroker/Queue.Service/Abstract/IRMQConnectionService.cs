using Queue.Data.Data.Resources.Enum;
using RabbitMQ.Client;

namespace Queue.Service.Abstract
{
    public interface IRMQConnectionService
    {
        IModel CreateChannel();
        IModel CreateChannel(string queueName);
        IModel CreateChannel(string exchangeName, string queueName, ExchType exchangeType, string routingKey);
        IModel CreateChannel(string exchangeName, ExchType exchangeType);
        void Dispose();
    }
}
