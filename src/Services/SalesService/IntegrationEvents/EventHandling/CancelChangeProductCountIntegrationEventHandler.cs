using EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using SalesService.Dtos;
using SalesService.IntegrationEvents.Events;
using SalesService.Services;
using System;
using System.Threading.Tasks;

namespace SalesService.IntegrationEvents.EventHandling
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
                    Name = @event.ProductName,
                    DecreaseCount = @event.Quantity
                };

                await _productService.CancelChangeProductCountAsync(productDto);

                //To do: delete order and orderitem

            }
            catch (ArgumentNullException ex)
            {
                _logger.LogInformation($"CancelChangeProductCountIntegrationEvent is null. Exception detail:{ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Product {@event.ProductName} has been Canceled. Exception detail:{ex.Message}");
                throw;
            }
        }
    }
}
