using EventBus.Events;

namespace ProductCatalogService.IntegrationEvents.Events
{
    public class InventoryResultIntegrationEvent : IntegrationEvent
    {
        public InventoryResultIntegrationEvent(int productId, bool isSuccess, string correlationId) : base(correlationId)
        {
            ProductId = productId;
            IsSuccess = isSuccess;
            CorrelationId = correlationId;
        }

        public int ProductId { get; private set; }
        public bool IsSuccess { get; private set; }
    }
}
