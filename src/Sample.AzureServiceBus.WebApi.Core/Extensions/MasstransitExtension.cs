using GreenPipes;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Sample.AzureServiceBus.WebApi.Core.Extensions
{
    public static class MasstransitExtension
    {
        public static void AddMassTransitExtension(this IServiceCollection services, IConfiguration configuration, bool configureConsumer = false)
        {
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                x.UsingAzureServiceBus((ctx, cfg) =>
                {
                    var connectionString = configuration.GetConnectionString("AzureServiceBus");
                    cfg.Host(connectionString);

                    cfg.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter("dev", false));
                    cfg.UseMessageRetry(retry => { retry.Interval(3, TimeSpan.FromSeconds(5)); });
                });
            });
            services.AddMassTransitHostedService();
        }
    }
}