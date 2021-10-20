using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Sample.AzureServiceBus.WebApi.Core.Models;

namespace Sample.AzureServiceBus.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IPublishEndpoint _publisher;
        private readonly ILogger<ClientController> _logger;

        public ClientController(ILogger<ClientController> logger, IPublishEndpoint publisher)
        {
            _logger = logger;
            _publisher = publisher;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ClientInsertedEvent insertedEvent)
        {
            await _publisher.Publish<ClientInsertedEvent>(insertedEvent);
            _logger.LogInformation($"Send client: {insertedEvent.ClientId} - {insertedEvent.Name}");

            return Ok();
        }
    }
}