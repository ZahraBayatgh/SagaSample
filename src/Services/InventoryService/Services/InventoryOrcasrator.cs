using CSharpFunctionalExtensions;
using EventBus.Abstractions;
using InventoryService.Data;
using InventoryService.Dtos;
using InventoryService.IntegrationEvents.Events;
using InventoryService.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InventoryService.Services
{
    public class InventoryOrcasrator : IInventoryOrcasrator
    {
        private readonly IProductService _productService;
        private readonly IInventoryTransactionService _inventoryTransactionService;
        private readonly ILogger<InventoryOrcasrator> _logger;
        private readonly IEventBus _eventBus;
        private readonly InventoryDbContext _context;

        public InventoryOrcasrator(IProductService productService,
             IInventoryTransactionService inventoryTransactionService,
             ILogger<InventoryOrcasrator> logger,
             IEventBus eventBus,
             InventoryDbContext inventoryDbContext
             )
        {
            _productService = productService;
            _inventoryTransactionService = inventoryTransactionService;
            _logger = logger;
            _eventBus = eventBus;
            _context = inventoryDbContext;
        }
        public async Task<Result<int>> CreateProductAndInventoryTransactionAsync(ProductDto productDto)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                // Check product instance
                var productValidation = CheckProductDtoInstance(productDto);
                if (productValidation.IsFailure)
                    return Result.Failure<int>(productValidation.Error);

                var createProductResponse = await _productService.CreateProductAsync(productDto);
                if (createProductResponse.IsFailure)
                    return Result.Failure<int>($"Product with {productDto.ProductName} name was not create.");

                InventoryTransactionDto inventoryTransactionDto = new InventoryTransactionDto
                {
                    ProductId = createProductResponse.Value.ProductId,
                    ChangeCount = createProductResponse.Value.ChangeCount,
                    CurrentCount = createProductResponse.Value.CurrentCount,
                    Type = InventoryType.In
                };

                var _inventoryTransaction = await _inventoryTransactionService.CreateInventoryTransactionAsync(inventoryTransactionDto);
                if (createProductResponse.IsFailure)
                {
                    transaction.Rollback();
                    return Result.Failure<int>($"InventoryTransaction with {createProductResponse.Value.ProductId} product id was not create.");
                }

                CreateProductIntegrationEvent createProductIntegrationEvent = new CreateProductIntegrationEvent(createProductResponse.Value.ProductId, _inventoryTransaction.Value.Id, createProductResponse.Value.ProductName, createProductResponse.Value.CurrentCount);
                _eventBus.Publish(createProductIntegrationEvent);

                transaction.Commit();
                return Result.Success(createProductResponse.Value.ProductId);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Create Product InventoryTransaction has been canceled. Exception detail:{ex.Message}");
                return Result.Failure<int>("Create Product InventoryTransaction has been canceled.");
            }
        }

        /// <summary>
        /// This methode check a createProductDto instance
        /// </summary>
        /// <param name="createProductDto"></param>
        /// <returns></returns>
        private static Result CheckProductDtoInstance(ProductDto createProductDto)
        {
            if (createProductDto == null)
                return Result.Failure($"ProductDto instance is invalid.");

            if (string.IsNullOrEmpty(createProductDto.ProductName))
                return Result.Failure($"Product name is empty.");

            if (createProductDto.Count <= 0)
                return Result.Failure($"Product count is invaild.");

            return Result.Success();
        }
    }
}
