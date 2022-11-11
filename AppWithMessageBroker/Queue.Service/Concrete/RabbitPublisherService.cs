using Core.Configurations.RabbitMq;
using Microsoft.Extensions.Options;
using Queue.Data.RabbitMQ;
using Queue.Service.Abstract;

namespace Queue.Service.Concrete
{
    public class RabbitPublisherService : RabbitMqPublisher, IPublisherService
    {
        public RabbitPublisherService(IOptions<RabbitConfig> config):base(config)
        {
        }
    }
}
