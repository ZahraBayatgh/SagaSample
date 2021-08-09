using EventBus.Events;

namespace InventoryService.IntegrationEvents.Events
{
    public class UpdateProductIntegrationEvent : IntegrationEvent
    {
        public UpdateProductIntegrationEvent(string name, int decreaseCount, int currentCount)
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
