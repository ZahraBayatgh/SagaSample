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
    public class UpdateProductCountAndAddInventoryTransactionEventHandler : IIntegrationEventHandler<UpdateProductCountAndAddInventoryTransactionEvent>
    {
        private readonly InventoryDbContext _context;
        private readonly ILogger<UpdateProductCountAndAddInventoryTransactionEvent> _logger;
        private readonly IProductService _productService;
        private readonly IInventoryTransactionService _inventoryTransactionService;
        private readonly IEventBus _eventBus;

        public UpdateProductCountAndAddInventoryTransactionEventHandler(InventoryDbContext context,
            ILogger<UpdateProductCountAndAddInventoryTransactionEvent> logger,
            IProductService productService,
            IInventoryTransactionService inventoryService,
            IEventBus eventBus)
        {
            _context = context;
            _logger = logger;
            _productService = productService;
            _inventoryTransactionService = inventoryService;
            _eventBus = eventBus;
        }

        public async Task Handle(UpdateProductCountAndAddInventoryTransactionEvent @event)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // Check event is null
                if (@event == null)
                    throw new ArgumentNullException("UpdateProductCountAndAddInventoryTransactionEvent is null.");

                bool isCommit = false;

                // Get product id
                var productId = await _productService.GetProductIdAsync(@event.Name);

                // Get latest InventoryTransaction by product id
                var latestInventoryTransactionCount = await _inventoryTransactionService.GetLatestInventoryTransactionByProductIdAsync(productId.Value);

                // Check latest InventoryTransaction
                if (latestInventoryTransactionCount.IsFailure)
                    throw new Exception(latestInventoryTransactionCount.Error);

                // Intialize InventoryTransactionDto
                var inventoryTransaction = new InventoryTransactionDto
                {
                    ProductId = productId.Value,
                    ChangeCount = @event.DecreaseCount,
                    CurrentCount = latestInventoryTransactionCount.Value - @event.DecreaseCount
                };

                // Create InventoryTransaction
                var inventoryTransactionResult = await _inventoryTransactionService.CreateInventoryTransactionAsync(inventoryTransaction);

                if (inventoryTransactionResult.IsSuccess)
                {
                    // Intialize ProductDto
                    var productDto = new ProductDto
                    {
                        ProductName = @event.Name,
                        Count = inventoryTransactionResult.Value.CurrentCount
                    };

                    // Update product
                    var product = await _productService.UpdateProductAsync(productDto);
                    if (product.IsSuccess)
                        isCommit = true;
                }

                if (isCommit)
                {
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                }
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogInformation($"UpdateProductCountAndAddInventoryTransactionEvent is null. Exception detail:{ex.Message}");
                throw;
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
