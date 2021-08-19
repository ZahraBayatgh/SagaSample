using EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using ProductCatalogService.IntegrationEvents.Events;
using ProductCatalogService.Models;
using ProductCatalogService.Services;
using SaleService.Dtos;
using System;
using System.Threading.Tasks;

namespace ProductCatalogService.IntegrationEvents.EventHandling
{
    public class SalesResultIntegrationEventHandler : IIntegrationEventHandler<SalesResultIntegrationEvent>
    {
        private readonly ILogger<SalesResultIntegrationEventHandler> _logger;
        private readonly IProductService _productService;
        private readonly IEventBus _eventBus;

        public SalesResultIntegrationEventHandler(ILogger<SalesResultIntegrationEventHandler> logger,
            IProductService productService,
            IEventBus eventBus)
        {
            _logger = logger;
            _productService = productService;
            _eventBus = eventBus;
        }

        public async Task Handle(SalesResultIntegrationEvent @event)
        {
            try
            {
                // Check SalesResultIntegrationEvent
                CheckSalesResultIntegrationEventInstance(@event);

                // Get and Check product in db
                var getProduct = await _productService.GetProductByIdAsync(@event.ProductId);
                if (getProduct.IsFailure && @event.IsSuccess)
                {
                    // Publish DeleteInventoryIntegrationEvent
                    DeleteSalesIntegrationEvent deleteInventoryIntegrationEvent = new DeleteSalesIntegrationEvent(getProduct.Value.Name, @event.CorrelationId);
                    await _eventBus.PublishAsync(deleteInventoryIntegrationEvent);

                }
                else if (getProduct.IsSuccess && @event.IsSuccess && (int)getProduct.Value.ProductStatus != (int)ProductStatus.SalesIsOk)
                {
                    var productStatus = (int)ProductStatus.SalesIsOk + (int)getProduct.Value.ProductStatus;
                    UpdateProductStatusRequestDto updateProductStatusRequestDto = new UpdateProductStatusRequestDto(getProduct.Value.Name, productStatus);

                    await _productService.UpdateProductStatusAsync(updateProductStatusRequestDto);
                }
                if (getProduct.IsSuccess && !@event.IsSuccess && getProduct.Value.ProductStatus == ProductStatus.InventoryIsOk)
                {
                    // Publish DeleteInventoryIntegrationEvent
                    DeleteInventoryIntegrationEvent deleteInventoryIntegrationEvent = new DeleteInventoryIntegrationEvent(getProduct.Value.Name, @event.CorrelationId);
                    await _eventBus.PublishAsync(deleteInventoryIntegrationEvent);

                    // Delete product
                    await _productService.DeleteProductAsync(getProduct.Value.Id);
                }
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogInformation($"SalesResultIntegrationEvent faild. {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"SalesResultIntegrationEvent with {@event.Id} product id failed. Exception detail:{ex.Message}");
                throw;
            }
        }

        private static void CheckSalesResultIntegrationEventInstance(SalesResultIntegrationEvent @event)
        {
            if (@event == null)
                throw new ArgumentNullException("SalesResultIntegrationEvent is null.");

            if (@event.ProductId <= 0)
                throw new ArgumentNullException("SalesResultIntegrationEvent ProductId is invalid.");
        }
    }
}
