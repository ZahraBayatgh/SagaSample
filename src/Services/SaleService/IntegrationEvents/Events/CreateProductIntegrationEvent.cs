using EventBus.Events;

namespace SaleService.IntegrationEvents.Events
{
    public class CreateProductIntegrationEvent : IntegrationEvent
    {
        public CreateProductIntegrationEvent(int productId, string productName, int initialOnHand)
        {
            ProductId = productId;
            ProductName = productName;
            InitialOnHand = initialOnHand;
        }

        public int ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int InitialOnHand { get; private set; }
    }
}
