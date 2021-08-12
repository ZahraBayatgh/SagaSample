using EventBus.Events;

namespace InventoryService.IntegrationEvents.Events
{
    public class DeleteProductIntegrationEvent : IntegrationEvent
    {
        public DeleteProductIntegrationEvent(int productId, int inventoryTransactionId)
        {
            ProductId = productId;
            InventoryTransactionId = inventoryTransactionId;
        }
        public int ProductId { get; set; }
        public int InventoryTransactionId { get; }
    }
}
