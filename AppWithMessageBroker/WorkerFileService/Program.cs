using Azure.Abstracts.Configs;
using Core.Azure.Config;
using Core.Configurations.RabbitMq;
using Queue.Data.Abstract;
using Queue.Service.Abstract;
using Queue.Service.Concrete;
using Services.Abstract.Cloud;
using Services.Concrete.Cloud;
using WorkerFileService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration Configuration = hostContext.Configuration;


        services.AddSingleton<IAzureConfig, AzureConfig>();
        services.Configure<AzureConfig>(Configuration);
        services.AddSingleton<IBlobService, BlobService>();

        services.AddSingleton<IRabbitMqConfig, RabbitConfig>();
        services.Configure<RabbitConfig>(Configuration);
        services.AddSingleton<IRMQConnectionService, RMQConnectionManager>();


        services.AddHostedService<ImageEventWorker>();
    })
    .Build();

await host.RunAsync();
