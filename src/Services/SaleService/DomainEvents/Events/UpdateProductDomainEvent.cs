using MediatR;

namespace SaleService.DomainEvents.Events
{
    public class UpdateProductDomainEvent : INotification
    {
        public UpdateProductDomainEvent(int orderId,int orderItemId, string productName, int quantity)
        {
            OrderId = orderId;
            OrderItemId = orderItemId;
            ProductName = productName;
            Quantity = quantity;
        }

        public int OrderId { get; set; }
        public int OrderItemId { get; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }

    }
}
