using CSharpFunctionalExtensions;
using EventBus.Abstractions;
using InventoryService.Data;
using InventoryService.Dtos;
using InventoryService.IntegrationEvents.Events;
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
        public async Task<Result<int>> CreateProductInventoryTransactionAsync(ProductDto productDto)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var createProductResponse = await _productService.CreateProductAsync(productDto);

                InventoryTransactionDto inventoryTransactionDto = new InventoryTransactionDto
                {
                    ProductId = createProductResponse.Value.ProductId,
                    ChangeCount = createProductResponse.Value.ChangeCount,
                    CurrentCount = createProductResponse.Value.CurrentCount
                };

                var _inventoryTransaction = await _inventoryTransactionService.CreateInventoryTransactionAsync(inventoryTransactionDto);

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
    }
}
