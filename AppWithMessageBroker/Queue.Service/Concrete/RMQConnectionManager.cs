using Core.Configurations.RabbitMq;
using Microsoft.Extensions.Options;
using Queue.Data.Data.Resources;
using Queue.Data.Data.Resources.Enum;
using Queue.Data.RabbitMQ;
using Queue.Service.Abstract;

namespace Queue.Service.Concrete
{
    public class RMQConnectionManager : QueueConnectionRepository, IRMQConnectionService
    {
        public RMQConnectionManager(IOptions<RabbitConfig> config) : base(config)
        {
        }
    }
}
