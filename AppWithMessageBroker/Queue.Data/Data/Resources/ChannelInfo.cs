using Queue.Data.Data.Resources.Enum;
using RabbitMQ.Client;

namespace Queue.Data.Data.Resources
{
    public class ChannelInfo : IDisposable
    {
        public ChannelInfo()
        {
            Id = Guid.NewGuid().ToString().Replace("-", string.Empty);
        }
        public IModel Channel { get; set; }
        public string Id { get; set; }
        public string ChannelName { get; set; }
        public string QueueName { get; set; }
        public string ExchangeName { get; set; }
        public string RoutingKey { get; set; }
        public ExchType ExchangeType { get; set; }

        public void Dispose()
        {
            this.Channel?.Close();
            this.Channel?.Dispose();
        }
    }
}
