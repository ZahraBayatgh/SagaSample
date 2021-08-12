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
        private readonly ILogger<UpdateProductCountAndAddInventoryTransaction> _logger;
        private readonly IProductService _productService;
        private readonly IInventoryTransactionService _inventoryService;

        public DeleteProductIntegrationEventHandler(InventoryDbContext context,
            ILogger<UpdateProductCountAndAddInventoryTransaction> logger,
            IProductService productService,
            IInventoryTransactionService inventoryService)
        {
            _context = context;
            _logger = logger;
            _productService = productService;
            _inventoryService = inventoryService;
        }

        public async Task Handle(DeleteProductIntegrationEvent @event)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // Delete InventoryTransaction
                var inventoryTransactionResult = await _inventoryService.DeleteInventoryTransactionAsync(@event.InventoryTransactionId);

                // Delete product
                var product = await _productService.DeleteProductAsync(@event.ProductId);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Product{@event.Id} has been deleted. Exception detail:{ex.Message}");
            }
        }
    }
}
