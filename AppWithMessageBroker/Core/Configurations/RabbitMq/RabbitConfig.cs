using Queue.Data.Abstract;

namespace Core.Configurations.RabbitMq
{
    public class RabbitConfig : IRabbitMqConfig
    {
        public string RMQ_CONNECTION_STRING { get; set; }
    }
}
