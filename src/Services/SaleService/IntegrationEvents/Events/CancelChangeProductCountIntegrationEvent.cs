using EventBus.Events;

namespace SaleService.IntegrationEvents.Events
{
    public class CancelChangeProductCountIntegrationEvent : IntegrationEvent
    {
        public CancelChangeProductCountIntegrationEvent(string productName, int quantity, int orderId, int orderItemId)
        {
            ProductName = productName;
            Quantity = quantity;
            OrderId = orderId;
            OrderItemId = orderItemId;
        }

        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public int OrderId { get; }
        public int OrderItemId { get; private set; }
    }
}
