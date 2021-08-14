using EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using SaleService.Dtos;
using SaleService.IntegrationEvents.Events;
using SaleService.Services;
using System;
using System.Threading.Tasks;

namespace SaleService.IntegrationEvents.EventHandling
{
    public class CancelChangeProductCountIntegrationEventHandler : IIntegrationEventHandler<CancelChangeProductCountIntegrationEvent>
    {
        private readonly ILogger<CancelChangeProductCountIntegrationEventHandler> _logger;
        private readonly IProductService _productService;

        public CancelChangeProductCountIntegrationEventHandler(ILogger<CancelChangeProductCountIntegrationEventHandler> logger,
            IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public async Task Handle(CancelChangeProductCountIntegrationEvent @event)
        {
            try
            {
                // Check event is null
                if (@event == null)
                    throw new ArgumentNullException("CancelChangeProductCountIntegrationEvent is null.");

                var productDto = new CancelChangeProductCountDto
                {
                    Name = @event.Name,
                    DecreaseCount = @event.DecreaseCount
                };

                await _productService.CancelChangeProductCountAsync(productDto);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogInformation($"CancelChangeProductCountIntegrationEvent is null. Exception detail:{ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Product {@event.Name} has been Canceled. Exception detail:{ex.Message}");
                throw;
            }
        }
    }
}
