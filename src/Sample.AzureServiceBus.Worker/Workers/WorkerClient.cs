using System;
using System.Threading.Tasks;
using MassTransit;
using Sample.AzureServiceBus.WebApi.Core.Models;

namespace Sample.AzureServiceBus.Worker.Workers
{
    public class WorkerClient : IConsumer<ClientModel>
    {
        public Task Consume(ConsumeContext<ClientModel> context)
        {
            var id = context.Message.ClientId;
            var name = context.Message.Name;

            Console.WriteLine($"Receive client: {id} - {name}");
            return Task.CompletedTask;
        }
    }
}