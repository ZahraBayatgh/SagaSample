using EventBus.Abstractions;
using InventoryService.Data;
using InventoryService.IntegrationEvents.Events;
using InventoryService.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InventoryService.IntegrationEvents.EventHandling
{
    public class DeleteProductIntegrationEventHandler : IIntegrationEventHandler<DeleteProductIntegrationEvent>
    {
        private readonly InventoryDbContext _context;
        private readonly ILogger<DeleteProductIntegrationEventHandler> _logger;
        private readonly IProductService _productService;
        private readonly IInventoryTransactionService _inventoryTransactionService;

        public DeleteProductIntegrationEventHandler(InventoryDbContext context,
            ILogger<DeleteProductIntegrationEventHandler> logger,
            IProductService productService,
            IInventoryTransactionService inventoryTransactionService)
        {
            _context = context;
            _logger = logger;
            _productService = productService;
            _inventoryTransactionService = inventoryTransactionService;
        }

        public async Task Handle(DeleteProductIntegrationEvent @event)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // Check event is null
                if (@event == null)
                    throw new ArgumentNullException("DeleteProductIntegrationEvent is null.");

                // Delete InventoryTransaction
                var inventoryTransactionResult = await _inventoryTransactionService.DeleteInventoryTransactionAsync(@event.InventoryTransactionId);
                if (inventoryTransactionResult.IsFailure)
                    throw new Exception($"InventoryTransaction with {@event.InventoryTransactionId} id was not deleted");

                // Delete product
                var product = await _productService.DeleteProductAsync(@event.ProductId);
                if (product.IsFailure)
                    throw new Exception($"Product with {@event.ProductId} id  was not deleted");

                transaction.Commit();
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogInformation($"DeleteProductIntegrationEvent is null. Exception detail:{ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Product{@event.Id} was not deleted. Exception detail:{ex.Message}");
                throw;
            }
        }
    }
}
