using Queue.Data.Data.Resources.Enum;

namespace Queue.Data.Abstract
{
    public interface IQueuePublisher
    {
        void Publish<T>(T eventBody, string queueName = "", string exchangeName = "", string routingKey = "", ExchType exchType = ExchType.direct) where T : class, IEventBody, new();

        void Publish(Stream stream, string queueName = "", string exchangeName = "", string routingKey = "", ExchType exchType = ExchType.direct);
    }
}
