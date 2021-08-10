using EventBus.Events;

namespace SaleService.IntegrationEvents.Events
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
