using EventBus.Events;

namespace InventoryService.IntegrationEvents.Events
{
    public class DeleteInventoryIntegrationEvent : IntegrationEvent
    {
        public DeleteInventoryIntegrationEvent(string productName)
        {
            ProductName = productName;
        }

        public string ProductName { get; private set; }
    }
}
