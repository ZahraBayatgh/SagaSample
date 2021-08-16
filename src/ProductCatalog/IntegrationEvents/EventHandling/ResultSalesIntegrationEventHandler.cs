using CSharpFunctionalExtensions;
using EventBus.Abstractions;
using EventBus.Events;
using Microsoft.Extensions.Logging;
using ProductCatalogService.IntegrationEvents.Events;
using ProductCatalogService.Models;
using ProductCatalogService.Services;
using SaleService.Dtos;
using System;
using System.Threading.Tasks;

namespace ProductCatalogService.IntegrationEvents.EventHandling
{
    public class ResultSalesIntegrationEventHandler : IIntegrationEventHandler<ResultSalesIntegrationEvent>
    {
        private readonly ILogger<ResultSalesIntegrationEventHandler> _logger;
        private readonly IProductService _productService;
        private readonly IEventBus _eventBus;

        public ResultSalesIntegrationEventHandler(ILogger<ResultSalesIntegrationEventHandler> logger,
            IProductService productService,
            IEventBus eventBus)
        {
            _logger = logger;
            _productService = productService;
            _eventBus = eventBus;
        }

        public async Task Handle(ResultSalesIntegrationEvent @event)
        {
            try
            {
                // Check ResultSalesIntegrationEvent
                CheckResultSalesIntegrationEventInstance(@event);

                // Get and Check product in db
                var getProduct = await _productService.GetProductByIdAsync(@event.ProductId);
                if (getProduct.IsFailure)
                    throw new ArgumentNullException(getProduct.Error);

                if (!@event.IsSuccess)
                {
                    if (getProduct.Value.ProductStatus == ProductStatus.InventoryIsOk)
                    {
                        // Publish DeleteInventoryIntegrationEvent
                        DeleteInventoryIntegrationEvent deleteInventoryIntegrationEvent = new DeleteInventoryIntegrationEvent(getProduct.Value.Name);
                        _eventBus.Publish(deleteInventoryIntegrationEvent);
                    }

                    // Delete product
                    await _productService.DeleteProductAsync(getProduct.Value.Id);
                }
                else
                {
                    // Update ProductStatus
                    var productStatus = (int)ProductStatus.SalesIsOk;
                    if ((int)getProduct.Value.ProductStatus != (int)ProductStatus.SalesIsOk)
                    {
                        productStatus += (int)getProduct.Value.ProductStatus;

                    }
                    UpdateProductStatusRequestDto updateProductStatusRequestDto = new UpdateProductStatusRequestDto(getProduct.Value.Name, productStatus);

                    await _productService.UpdateProductStatusAsync(updateProductStatusRequestDto);
                }
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogInformation($"ResultSalesIntegrationEvent faild. {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"ResultSalesIntegrationEvent with {@event.Id} product id failed. Exception detail:{ex.Message}");
                throw;
            }
        }

        private static void CheckResultSalesIntegrationEventInstance(ResultSalesIntegrationEvent @event)
        {
            if (@event == null)
                throw new ArgumentNullException("ResultSalesIntegrationEvent is null.");

            if (@event.ProductId <= 0)
                throw new ArgumentNullException("ResultSalesIntegrationEvent is null.");
        }
    }
}
