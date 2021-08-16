using EventBus.Abstractions;
using InventoryService.Dtos;
using InventoryService.IntegrationEvents.Events;
using InventoryService.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InventoryService.IntegrationEvents.EventHandling
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
                // Check CreateProductIntegrationEvent
                CheckCreateProductIntegrationEventInstance(@event);

                // Create product
                var ProductRequestDto = new ProductRequestDto
                {
                    ProductName = @event.ProductName,
                    Count = @event.InitialOnHand
                };
                var createProductResponce = await _productService.CreateProductAsync(ProductRequestDto);

                // Publish ResultSalesIntegrationEvent
                bool createProductStatus = createProductResponce.IsSuccess ? true : false;
                PublishResult(@event, createProductStatus);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogInformation($"CreateProductIntegrationEvent is null. Exception detail:{ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Product {@event.ProductName} wan not created. Exception detail:{ex.Message}");

                // Publish ResultSalesIntegrationEvent
                PublishResult(@event, false);

                throw;
            }
        }

        private void PublishResult(CreateProductIntegrationEvent @event, bool createProductStatus)
        {
            // Publish ResultSalesIntegrationEvent
            ResultInventoryIntegrationEvent resultInventoryIntegrationEvent = new ResultInventoryIntegrationEvent(@event.ProductId, createProductStatus);
            _eventBus.Publish(resultInventoryIntegrationEvent);
        }

        private static void CheckCreateProductIntegrationEventInstance(CreateProductIntegrationEvent @event)
        {
            if (@event == null)
                throw new ArgumentNullException("CreateProductIntegrationEvent is null.");

            if (@event.ProductId <= 0)
                throw new ArgumentNullException("CreateProductIntegrationEvent ProductId is invalid.");

            if (string.IsNullOrEmpty(@event.ProductName))
                throw new ArgumentNullException("ResultSalesIntegrationEvent ProductName is null.");
        }
    }
}
