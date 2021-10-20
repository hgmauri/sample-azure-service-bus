using MassTransit;
using Sample.AzureServiceBus.WebApi.Core.Models;

namespace Sample.AzureServiceBus.Worker.Workers
{
    public class WorkerClientConsumer : IConsumer<ClientInsertedEvent>
    {
        public Task Consume(ConsumeContext<ClientInsertedEvent> context)
        {
            var id = context.Message.ClientId;
            var name = context.Message.Name;

            Console.WriteLine($"Receive client: {id} - {name}");
            return Task.CompletedTask;
        }
    }
}