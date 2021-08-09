using EventBus.Abstractions;
using InventoryService.Data;
using InventoryService.IntegrationEvents.Events;
using InventoryService.Models;
using InventoryService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.IntegrationEvents.EventHandling
{
    public class UpdateProductIntegrationEventHandler : IIntegrationEventHandler<UpdateProductIntegrationEvent>
    {
        private readonly ILogger<UpdateProductIntegrationEvent> _logger;
        private readonly IProductService _productService;
        private readonly IInventoryService _inventoryService;
        private readonly IEventBus _eventBus;

        public UpdateProductIntegrationEventHandler(ILogger<UpdateProductIntegrationEvent> logger,
            IProductService productService ,
            IInventoryService inventoryService,
            IEventBus eventBus)
        {
            _logger = logger;
            _productService = productService;
            _inventoryService = inventoryService;
            _eventBus = eventBus;
        }

        public async Task Handle(UpdateProductIntegrationEvent @event)
        {
            try
            {
                throw new System.Exception();
                var product = await _productService.UpdateProductAsync(new Dtos.ProductDto { Name = @event.Name, Count = @event.CurrentCount });

               await _inventoryService.AddInventoryAsync( new Inventory { ProductId = product.Id, Type = Type.Out, Count = @event.DecreaseCount });
            }
            catch (System.Exception)
            {
                _eventBus.Publish(new CancelProductIntegrationEvent(@event.Name, @event.CurrentCount+@event.DecreaseCount));
                _logger.LogInformation($"Product{@event.Name} has been canceled");
            }
        }
    }
}
