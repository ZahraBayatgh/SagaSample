using EventBus.Events;

namespace SaleService.IntegrationEvents.Events
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
