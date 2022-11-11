using Queue.Data.Abstract;
using Queue.Data.Data.Resources.Enum;

namespace Queue.Service.Abstract
{
    public interface IPublisherService
    {
        void Publish<T>(T eventBody, string queueName = "", string exchangeName = "", string routingKey = "", ExchType exchType = ExchType.direct) where T : class, IEventBody, new();

        void Publish(Stream stream, string queueName = "", string exchangeName = "", string routingKey = "", ExchType exchType = ExchType.direct);
    }
}
