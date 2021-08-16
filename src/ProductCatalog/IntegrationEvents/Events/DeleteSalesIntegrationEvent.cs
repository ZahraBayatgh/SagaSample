using EventBus.Events;

namespace ProductCatalogService.IntegrationEvents.Events
{
    public class DeleteInventoryIntegrationEvent: IntegrationEvent
    {
        public DeleteInventoryIntegrationEvent(string productName)
        {
            ProductName = productName;
        }

        public string ProductName { get; private set; }
    }
}
