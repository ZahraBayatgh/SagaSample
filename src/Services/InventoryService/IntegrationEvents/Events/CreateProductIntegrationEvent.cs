using EventBus.Events;

namespace InventoryService.IntegrationEvents.Events
{
    public class CreateProductIntegrationEvent : IntegrationEvent
    {
        public CreateProductIntegrationEvent(int productId, int inventoryTransactionId, string productName, int currentCount)
        {
            ProductId = productId;
            InventoryTransactionId = inventoryTransactionId;
            ProductName = productName;
            CurrentCount = currentCount;
        }

        public int ProductId { get; }
        public int InventoryTransactionId { get; }
        public string ProductName { get; }
        public int CurrentCount { get; }
    }
}
