using EventBus.Events;

namespace InventoryService.IntegrationEvents.Events
{
    public class CreateProductIntegrationEvent : IntegrationEvent
    {
        public CreateProductIntegrationEvent(int productId, string productName, int initialOnHand)
        {
            ProductId = productId;
            ProductName = productName;
            InitialOnHand = initialOnHand;
        }

        public int ProductId { get; }
        public string ProductName { get; }
        public int InitialOnHand { get; }
    }
}
