using EventBus.Events;

namespace SaleService.IntegrationEvents.Events
{
    public class CancelChangeProductCountIntegrationEvent : IntegrationEvent
    {
        public CancelChangeProductCountIntegrationEvent(string name, int count)
        {
            Name = name;
            DecreaseCount = count;
        }
        public string Name { get; set; }
        public int DecreaseCount { get; set; }
    }
}
