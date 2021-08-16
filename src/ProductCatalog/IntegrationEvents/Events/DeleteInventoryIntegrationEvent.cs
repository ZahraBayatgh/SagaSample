using EventBus.Events;

namespace ProductCatalogService.IntegrationEvents.Events
{
    public class DeleteSalesIntegrationEvent : IntegrationEvent
    {
        public DeleteSalesIntegrationEvent(string productName)
        {
            ProductName = productName;
        }

        public string ProductName { get; private set; }
    }
}
