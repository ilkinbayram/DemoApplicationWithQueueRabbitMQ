using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using UdemyWorkerService.Services;

namespace UdemyWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly RabbitMqClientService _rabbitMqClientService;
        private IModel _channel;

        public Worker(ILogger<Worker> logger, RabbitMqClientService rabbitMqClientService)
        {
            _logger = logger;
            _rabbitMqClientService = rabbitMqClientService;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _channel = _rabbitMqClientService.Connect();
            _channel.BasicQos(0, 1, false);
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            _channel.BasicConsume("fatal-logs-queue", false, consumer);
            consumer.Received += Consumer_Received;
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            await Task.Delay(1000);
            var receivedMessage = Encoding.UTF8.GetString(@event.Body.ToArray());
            Console.WriteLine($"{receivedMessage}");

            _channel.BasicAck(@event.DeliveryTag, false);
        }
    }
}