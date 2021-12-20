using GreenPipes;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;
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

    public class WorkerClientConsumerDefinition : ConsumerDefinition<WorkerClientConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<WorkerClientConsumer> consumerConfigurator)
        {
            consumerConfigurator.UseMessageRetry(retry => retry.Interval(3, TimeSpan.FromSeconds(5)));
        }
    }
}