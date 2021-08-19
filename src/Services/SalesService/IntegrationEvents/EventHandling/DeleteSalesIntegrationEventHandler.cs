using EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using SalesService.IntegrationEvents.Events;
using SalesService.Services;
using System;
using System.Threading.Tasks;

namespace SalesService.IntegrationEvents.EventHandling
{
    public class DeleteSalesIntegrationEventHandler : IIntegrationEventHandler<DeleteSalesIntegrationEvent>
    {
        private readonly ILogger<DeleteSalesIntegrationEventHandler> _logger;
        private readonly IProductService _productService;

        public DeleteSalesIntegrationEventHandler(ILogger<DeleteSalesIntegrationEventHandler> logger,
            IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }
        public async Task Handle(DeleteSalesIntegrationEvent @event)
        {
            try
            {
                // Check CreateProductIntegrationEvent
                CheckDeleteSalesIntegrationEventInstance(@event);

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

        private static void CheckDeleteSalesIntegrationEventInstance(DeleteSalesIntegrationEvent @event)
        {
            if (@event == null)
                throw new ArgumentNullException("CreateProductIntegrationEvent is null.");

            if (string.IsNullOrEmpty(@event.ProductName))
                throw new ArgumentNullException("ResultSalesIntegrationEvent ProductName is null.");
        }
    }
}
