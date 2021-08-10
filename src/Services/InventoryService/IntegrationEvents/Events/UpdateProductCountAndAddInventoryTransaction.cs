using EventBus.Events;

namespace InventoryService.IntegrationEvents.Events
{
    public class UpdateProductCountAndAddInventoryTransaction : IntegrationEvent
    {
        public UpdateProductCountAndAddInventoryTransaction(string name, int decreaseCount)
        {
            Name = name;
            DecreaseCount = decreaseCount;
        }
        public string Name { get; set; }
        public int DecreaseCount { get; set; }
    }
}
