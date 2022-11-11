using Queue.Data.Abstract;
using Queue.Data.Data.Resources;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace WorkerFileService
{
    public class MessageWorkerWithMemory : BackgroundService
    {
        private readonly ILogger<MessageWorkerWithMemory> _logger;
        private readonly IQueueConnection _connectionService;
        private readonly string _queueName;
        private IModel _channel;

        public MessageWorkerWithMemory(ILogger<MessageWorkerWithMemory> logger, 
                                       IQueueConnection connectionService, 
                                       string queueName)
        {
            _logger = logger;
            _connectionService = connectionService;
            _queueName = queueName;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _channel = _connectionService.CreateChannel();
            _channel.BasicQos(0, 1, true);
            return Task.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            _channel.BasicConsume(_queueName, false, consumer);

            consumer.Received += Consumer_Received;
        }

        private Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            using MemoryStream ms = new(@event.Body.ToArray());
            Thread.Sleep(20000);
            _channel.BasicAck(@event.DeliveryTag, false);
            return Task.CompletedTask;
        }
    }
}