using GreenPipes;
using MassTransit;
using Sample.AzureServiceBus.WebApi.Core.Abstractions;
using Sample.AzureServiceBus.WebApi.Core.Models;
using Sample.AzureServiceBus.Worker.Workers;

var configuration = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .AddJsonFile("appsettings.json")
    .Build();

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(builder =>
    {
        builder.Sources.Clear();
        builder.AddConfiguration(configuration);
    })
    .ConfigureServices((context, collection) =>
    {
        collection.AddMassTransit(x =>
        {
            x.AddConsumer<WorkerClientConsumer>();

            x.UsingAzureServiceBus((context, cfg) =>
            {
                cfg.Host(configuration.GetConnectionString("AzureServiceBus"), h =>
                {
                    h.RetryLimit = 3;
                    h.OperationTimeout = TimeSpan.FromSeconds(30);
                });

                cfg.Message<ClientInsertedEvent>(conf => conf.SetEntityName(BusMessagens.PublishClientInserted));

                cfg.SubscriptionEndpoint<ClientInsertedEvent>(BusMessagens.SubscribeClientInserted, conf =>
                {
                    conf.UseMessageRetry(r => r.Immediate(1));
                    conf.ConfigureConsumer<WorkerClientConsumer>(context);
                });
            });
        });
        collection.AddMassTransitHostedService(true);
    })
    .Build();

host.StartAsync();

Console.WriteLine("Waiting for new messages.");

while (true) ;
