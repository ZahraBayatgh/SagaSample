using EventBus.Events;

namespace InventoryService.IntegrationEvents.Events
{
    public class ResultInventoryIntegrationEvent : IntegrationEvent
    {
        public ResultInventoryIntegrationEvent(int productId, bool isSuccess, string correlationId) : base(correlationId)
        {
            ProductId = productId;
            IsSuccess = isSuccess;
            CorrelationId = correlationId;
        }

        public int ProductId { get; private set; }
        public bool IsSuccess { get; private set; }
    }
}
