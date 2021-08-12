using EventBus.Events;

namespace SaleService.IntegrationEvents.Events
{
    public class DeleteProductIntegrationEvent : IntegrationEvent
    {
        public DeleteProductIntegrationEvent(int productId,int inventoryTransactionId)
        {
            ProductId = productId;
            InventoryTransactionId = inventoryTransactionId;
        }
        public int ProductId { get; set; }
        public int InventoryTransactionId { get; }
    }
}
