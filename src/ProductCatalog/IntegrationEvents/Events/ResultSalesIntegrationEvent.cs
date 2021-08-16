using EventBus.Events;

namespace ProductCatalogService.IntegrationEvents.Events
{
    public class ResultSalesIntegrationEvent: IntegrationEvent
    {
        public ResultSalesIntegrationEvent(int productId, bool isSuccess)
        {
            ProductId = productId;
            IsSuccess = isSuccess;
        }

        public int ProductId { get; private set; }
        public bool IsSuccess { get; private set; }
    }
}
