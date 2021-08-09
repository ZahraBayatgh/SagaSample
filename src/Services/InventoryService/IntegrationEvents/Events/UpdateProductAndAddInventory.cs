using EventBus.Events;

namespace InventoryService.IntegrationEvents.Events
{
    public class UpdateProductAndAddInventory : IntegrationEvent
    {
        public UpdateProductAndAddInventory(string name, int decreaseCount, int currentCount)
        {
            Name = name;
            DecreaseCount = decreaseCount;
            CurrentCount = currentCount;
        }
        public string Name { get; set; }
        public int CurrentCount { get; set; }
        public int DecreaseCount { get; set; }
    }
}
