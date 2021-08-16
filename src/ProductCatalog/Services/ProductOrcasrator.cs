using CSharpFunctionalExtensions;
using EventBus.Abstractions;
using InventoryService.IntegrationEvents.Events;
using Microsoft.Extensions.Logging;
using ProductCatalogService.Services;
using System;

namespace InventoryService.Services
{
    public class ProductOrcasrator
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductOrcasrator> _logger;
        private readonly IEventBus _eventBus;

        public ProductOrcasrator(IProductService productService,
             ILogger<ProductOrcasrator> logger,
             IEventBus eventBus)
        {
            _productService = productService;
            _logger = logger;
            _eventBus = eventBus;
        }
        public  Result PublishProduct(CreateProductIntegrationEvent createProductIntegrationEvent)
        {
            try
            {
                // Check product instance
                var productValidation = CheckProductDtoInstance(createProductIntegrationEvent);
                if (productValidation.IsFailure)
                    return Result.Failure<int>(productValidation.Error);

                // Publish createProductIntegrationEvent
                _eventBus.Publish(createProductIntegrationEvent);

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Publish Product has been canceled. Exception detail:{ex.Message}");

                _productService.DeleteProductAsync(createProductIntegrationEvent.ProductId);

                return Result.Failure("Publish Product has been canceled.");
            }
        }

        /// <summary>
        /// This methode check a createProductDto instance
        /// </summary>
        /// <param name="createProductDto"></param>
        /// <returns></returns>
        private static Result CheckProductDtoInstance(CreateProductIntegrationEvent createProductIntegrationEvent)
        {
            if (createProductIntegrationEvent == null)
                return Result.Failure($"CreateProductIntegrationEvent instance is null.");

            if (createProductIntegrationEvent.ProductId<=0)
                return Result.Failure($"Product id is invalid."); 
            
            if (string.IsNullOrEmpty(createProductIntegrationEvent.ProductName))
                return Result.Failure($"Product name is empty.");

            if (createProductIntegrationEvent.InitialOnHand <= 0)
                return Result.Failure($"Product count is invaild.");

            return Result.Success();
        }
    }
}
