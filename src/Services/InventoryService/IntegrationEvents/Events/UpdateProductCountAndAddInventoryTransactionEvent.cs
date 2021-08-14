using EventBus.Events;

namespace InventoryService.IntegrationEvents.Events
{
    public class UpdateProductCountAndAddInventoryTransactionEvent : IntegrationEvent
    {
        public UpdateProductCountAndAddInventoryTransactionEvent(string name, int decreaseCount)
        {
            Name = name;
            DecreaseCount = decreaseCount;
        }
        public string Name { get; }
        public int DecreaseCount { get; }
    }
}
