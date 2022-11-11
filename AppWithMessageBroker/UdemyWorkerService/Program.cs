using RabbitMQ.Client;
using UdemyWorkerService;
using UdemyWorkerService.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton<RabbitMqClientService>();
        services.AddSingleton(sp => new ConnectionFactory() { Uri = new Uri("amqps://qdgpwfhh:axPjccfi9MQYMStsfxGtMlxIn3T5Gf6B@possum.lmq.cloudamqp.com/qdgpwfhh"), DispatchConsumersAsync = true });
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
