using EventBus.Abstractions;
using InventoryService.IntegrationEvents.Events;
using InventoryService.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InventoryService.IntegrationEvents.EventHandling
{
    public class DeleteInventoryIntegrationEventHandler : IIntegrationEventHandler<DeleteInventoryIntegrationEvent>
    {
        private readonly ILogger<DeleteInventoryIntegrationEventHandler> _logger;
        private readonly IProductService _productService;

        public DeleteInventoryIntegrationEventHandler(ILogger<DeleteInventoryIntegrationEventHandler> logger,
            IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }
        public async Task Handle(DeleteInventoryIntegrationEvent @event)
        {
            try
            {
                // Check CreateProductIntegrationEvent
                CheckDeleteInventoryIntegrationEventInstance(@event);

                // Get and Check product in db
                var getProduct = await _productService.GetProductByNameAsync(@event.ProductName);
                if (getProduct.IsFailure)
                    throw new ArgumentNullException(getProduct.Error);

                // Delete product
                var createProductResponce = await _productService.DeleteProductAsync(getProduct.Value.Id);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogInformation($"CreateProductIntegrationEvent is null. Exception detail:{ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Product {@event.ProductName} wan not created. Exception detail:{ex.Message}");


                throw;
            }
        }

        private static void CheckDeleteInventoryIntegrationEventInstance(DeleteInventoryIntegrationEvent @event)
        {
            if (@event == null)
                throw new ArgumentNullException("CreateProductIntegrationEvent is null.");

            if (string.IsNullOrEmpty(@event.ProductName))
                throw new ArgumentNullException("ResultSalesIntegrationEvent ProductName is null.");
        }
    }
}
