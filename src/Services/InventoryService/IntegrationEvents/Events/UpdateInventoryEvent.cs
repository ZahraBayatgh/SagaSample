using EventBus.Events;

namespace InventoryService.IntegrationEvents.Events
{
    public class UpdateInventoryEvent : IntegrationEvent
    {
        public UpdateInventoryEvent(string productName, int quantity, int orderId, int orderItemId, string correlationId) : base(correlationId)
        {
            ProductName = productName;
            Quantity = quantity;
            OrderId = orderId;
            OrderItemId = orderItemId;
            CorrelationId = correlationId;
        }

        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public int OrderId { get; }
        public int OrderItemId { get; private set; }
    }
}
