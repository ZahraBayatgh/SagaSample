using EventBus.Events;

namespace SaleService.IntegrationEvents.Events
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
