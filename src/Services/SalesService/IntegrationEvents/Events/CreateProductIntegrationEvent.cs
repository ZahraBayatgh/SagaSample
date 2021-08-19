using EventBus.Events;

namespace SalesService.IntegrationEvents.Events
{
    public class CreateProductIntegrationEvent : IntegrationEvent
    {
        public CreateProductIntegrationEvent(int productId, string productName, int initialOnHand, string correlationId) : base(correlationId)
        {
            ProductId = productId;
            ProductName = productName;
            InitialOnHand = initialOnHand;
            CorrelationId = correlationId;
        }

        public int ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int InitialOnHand { get; private set; }
    }
}
