using GreenPipes;
using MassTransit;
using Sample.AzureServiceBus.WebApi.Core.Abstractions;
using Sample.AzureServiceBus.WebApi.Core.Models;
using Sample.AzureServiceBus.Worker.Workers;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, collection) =>
    {
        collection.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            x.AddConsumer<WorkerClientConsumer>(typeof(WorkerClientConsumerDefinition));

            x.UsingAzureServiceBus((ctx, cfg) =>
            {
                cfg.Host(context.Configuration.GetConnectionString("AzureServiceBus"), h => h.RetryLimit = 3);

                cfg.Message<ClientInsertedEvent>(conf => conf.SetEntityName(BusMessagens.PublishClientInserted));

                cfg.SubscriptionEndpoint<ClientInsertedEvent>(BusMessagens.SubscribeClientInserted, conf =>
                {
                    conf.UseMessageRetry(r => r.Immediate(1));
                    conf.ConfigureConsumer<WorkerClientConsumer>(ctx);
                });
            });
        });
        collection.AddMassTransitHostedService(true);
    })
    .Build();

host.StartAsync();

Console.WriteLine("Waiting for new messages.");

while (true) ;
