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
    public class UpdateProductAndAddInventoryIntegrationEventHandler : IIntegrationEventHandler<UpdateProductAndAddInventory>
    {
        private readonly InventoryDbContext _context;
        private readonly ILogger<UpdateProductAndAddInventory> _logger;
        private readonly IProductService _productService;
        private readonly IInventoryService _inventoryService;
        private readonly IEventBus _eventBus;

        public UpdateProductAndAddInventoryIntegrationEventHandler(InventoryDbContext context ,
            ILogger<UpdateProductAndAddInventory> logger,
            IProductService productService ,
            IInventoryService inventoryService,
            IEventBus eventBus)
        {
            _context = context;
            _logger = logger;
            _productService = productService;
            _inventoryService = inventoryService;
            _eventBus = eventBus;
        }

        public async Task Handle(UpdateProductAndAddInventory @event)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var productDto = new ProductDto
                { 
                    Name = @event.Name,
                    Count = @event.CurrentCount 
                };
                var product = await _productService.UpdateProductAsync(productDto);

                var inventory = new Inventory
                {
                    ProductId = product.Id,
                    Type = InventoryType.Out, 
                    Count = @event.DecreaseCount
                };
               await _inventoryService.AddInventoryAsync(inventory);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                var cancelProductIntegrationEvent = new CancelProductIntegrationEvent(@event.Name, @event.CurrentCount + @event.DecreaseCount);
                _eventBus.Publish(cancelProductIntegrationEvent);

                _logger.LogInformation($"Product{@event.Name} has been canceled. Exception detail:{ex.Message}");
            }
        }
    }
}
