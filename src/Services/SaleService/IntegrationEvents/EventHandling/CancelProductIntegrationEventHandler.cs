using EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using SaleService.Dtos;
using SaleService.IntegrationEvents.Events;
using SaleService.Models;
using SaleService.Services;
using System.Threading.Tasks;

namespace SaleService.IntegrationEvents.EventHandling
{
    public class CancelProductIntegrationEventHandler : IIntegrationEventHandler<CancelProductIntegrationEvent>
    {
        private readonly ILogger<CancelProductIntegrationEventHandler> _logger;
        private readonly IProductService _productService;

        public CancelProductIntegrationEventHandler(ILogger<CancelProductIntegrationEventHandler> logger,
            IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public async Task Handle(CancelProductIntegrationEvent @event)
        {
            try
            {
                await  _productService.CancelProductAsync(new ProductDto { Name = @event.Name, Count = @event.Count });
            }
            catch (System.Exception)
            {

                _logger.LogInformation($"Product {@event.Name} has been Canceled");
            }
        }
    }
}
