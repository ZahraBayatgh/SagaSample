using EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using SaleService.Dtos;
using SaleService.IntegrationEvents.Events;
using SaleService.Services;
using System;
using System.Threading.Tasks;

namespace SaleService.IntegrationEvents.EventHandling
{
    public class CreateProductIntegrationEventHandler : IIntegrationEventHandler<CreateProductIntegrationEvent>
    {
        private readonly ILogger<CreateProductIntegrationEventHandler> _logger;
        private readonly IProductService _productService;
        private readonly IEventBus _eventBus;

        public CreateProductIntegrationEventHandler(ILogger<CreateProductIntegrationEventHandler> logger,
            IProductService productService,
            IEventBus eventBus)
        {
            _logger = logger;
            _productService = productService;
            _eventBus = eventBus;
        }
        public async Task Handle(CreateProductIntegrationEvent @event)
        {
            try
            {
                var productDto = new CreateProductDto
                {
                    Name = @event.ProductName,
                    Count = @event.CurrentCount
                };

                await _productService.CreateProductAsync(productDto);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Product {@event.ProductName} wan not created. Exception detail:{ex.Message}");

                DeleteProductIntegrationEvent deleteProductIntegrationEvent = new DeleteProductIntegrationEvent(@event.ProductId,@event.InventoryTransactionId);
                _eventBus.Publish(deleteProductIntegrationEvent);
            }
        }
    }
}
