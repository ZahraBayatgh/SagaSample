using EventBus.Abstractions;
using InventoryService.Data;
using InventoryService.Dtos;
using InventoryService.IntegrationEvents.Events;
using InventoryService.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InventoryService.IntegrationEvents.EventHandling
{
    public class UpdateProductCountAndAddInventoryTransactionEventHandler : IIntegrationEventHandler<UpdateProductCountAndAddInventoryTransaction>
    {
        private readonly InventoryDbContext _context;
        private readonly ILogger<UpdateProductCountAndAddInventoryTransaction> _logger;
        private readonly IProductService _productService;
        private readonly IInventoryTransactionService _inventoryService;
        private readonly IEventBus _eventBus;

        public UpdateProductCountAndAddInventoryTransactionEventHandler(InventoryDbContext context,
            ILogger<UpdateProductCountAndAddInventoryTransaction> logger,
            IProductService productService,
            IInventoryTransactionService inventoryService,
            IEventBus eventBus)
        {
            _context = context;
            _logger = logger;
            _productService = productService;
            _inventoryService = inventoryService;
            _eventBus = eventBus;
        }

        public async Task Handle(UpdateProductCountAndAddInventoryTransaction @event)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // Get product id
                var productId = await _productService.GetProductIdAsync(@event.Name);

                // Get latest InventoryTransaction by product id
                var latestInventoryTransactionCount = await _inventoryService.GetLatestInventoryTransactionByProductIdAsync(productId.Value);

                // Intialize InventoryTransactionDto
                var inventoryTransaction = new InventoryTransactionDto
                {
                    ProductId = productId.Value,
                    ChangeCount = @event.DecreaseCount,
                    CurrentCount= latestInventoryTransactionCount.Value - @event.DecreaseCount
                };

                // Create InventoryTransaction
                var inventoryTransactionResult = await _inventoryService.CreateInventoryTransactionAsync(inventoryTransaction);

                // Intialize ProductDto
                var productDto = new ProductDto
                {
                    Name = @event.Name,
                    Count = inventoryTransactionResult.Value.CurrentCount
                };

                // Update product
                var product = await _productService.UpdateProductAsync(productDto);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                var cancelProductIntegrationEvent = new CancelChangeProductCountIntegrationEvent(@event.Name, @event.DecreaseCount);
                _eventBus.Publish(cancelProductIntegrationEvent);

                _logger.LogInformation($"Product{@event.Name} has been canceled. Exception detail:{ex.Message}");
            }
        }
    }
}
