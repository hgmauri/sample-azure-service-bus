using System;
using MassTransit;
using MassTransit.Azure.ServiceBus.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
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
            x.AddConsumer<WorkerClient>();
            x.AddBus(provider => AzureBusFactory.CreateUsingServiceBus(cfg =>
            {
                cfg.Host(configuration.GetConnectionString("AzureServiceBus"), h =>
                {
                    h.RetryLimit = 3;
                    h.OperationTimeout = TimeSpan.FromSeconds(30);
                });

                cfg.ReceiveEndpoint("subscription-clientmodel", c =>
                {
                    c.EnablePartitioning = true;
                    c.Consumer<WorkerClient>(provider);
                });
            }));
        });
        collection.AddMassTransitHostedService(true);
    })
    .Build();

host.StartAsync();

Console.WriteLine("Waiting for new messages.");

while (true);
