using MediatR;

namespace SaleService.DomainEvents.Events
{
    public class UpdateProductDomainEvent : INotification
    {
        public UpdateProductDomainEvent(int orderId,string name, int decreaseCount)
        {
            OrderId = orderId;
            Name = name;
            DecreaseCount = decreaseCount;
        }

        public int OrderId { get; set; }
        public string Name { get; set; }
        public int DecreaseCount { get; set; }

    }
}
