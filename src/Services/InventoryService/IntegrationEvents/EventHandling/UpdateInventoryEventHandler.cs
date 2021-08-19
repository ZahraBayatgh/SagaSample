using EventBus.Abstractions;
using InventoryService.Data;
using InventoryService.Dtos;
using InventoryService.IntegrationEvents.Events;
using InventoryService.Models;
using InventoryService.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InventoryService.IntegrationEvents.EventHandling
{
    public class UpdateInventoryEventHandler : IIntegrationEventHandler<UpdateInventoryEvent>
    {
        private readonly InventoryDbContext _context;
        private readonly ILogger<UpdateInventoryEvent> _logger;
        private readonly IProductService _productService;
        private readonly IInventoryTransactionService _inventoryTransactionService;
        private readonly IEventBus _eventBus;

        public UpdateInventoryEventHandler(InventoryDbContext context,
            ILogger<UpdateInventoryEvent> logger,
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

        public async Task Handle(UpdateInventoryEvent @event)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // Check event is null
                if (@event == null)
                    throw new ArgumentNullException("UpdateProductCountAndAddInventoryTransactionEvent is null.");

                bool isCommit = false;

                // Get product id
                var productId = await _productService.GetProductIdAsync(@event.ProductName);

                if (productId.IsFailure)
                    throw new ArgumentNullException("UpdateProductCountAndAddInventoryTransactionEvent product is invalid.");

                // Get latest InventoryTransaction by product id
                var latestInventoryTransactionCount = await _inventoryTransactionService.GetLatestInventoryTransactionByProductIdAsync(productId.Value);

                // Check latest InventoryTransaction
                if (latestInventoryTransactionCount.IsFailure)
                    throw new Exception(latestInventoryTransactionCount.Error);

                // Intialize InventoryTransactionDto
                var inventoryTransaction = new InventoryTransactionRequestDto(productId.Value, @event.Quantity, latestInventoryTransactionCount.Value - @event.Quantity, InventoryType.Out);


                // Create InventoryTransaction
                var inventoryTransactionResult = await _inventoryTransactionService.CreateInventoryTransactionAsync(inventoryTransaction);

                if (inventoryTransactionResult.IsSuccess)
                {
                    // Intialize ProductDto
                    var productRequestDto = new ProductRequestDto
                    {
                        ProductName = @event.ProductName,
                        Count = inventoryTransactionResult.Value.CurrentCount
                    };

                    // Update product
                    var product = await _productService.UpdateProductAsync(productRequestDto);
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
                var cancelProductIntegrationEvent = new CancelChangeProductCountIntegrationEvent(@event.ProductName, @event.Quantity,@event.OrderId,@event.OrderItemId, @event.CorrelationId);
               await _eventBus.PublishAsync(cancelProductIntegrationEvent);

                _logger.LogInformation($"Product{@event.ProductName} has been canceled. Exception detail:{ex.Message}");
            }
        }
    }
}
