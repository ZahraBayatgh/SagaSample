using MediatR;

namespace SaleService.DomainEvents.Events
{
    public class UpdateProductDomainEvent : INotification
    {
        public UpdateProductDomainEvent(int orderId,int orderItemId, string productName, int quantity,string correlationId)
        {
            OrderId = orderId;
            OrderItemId = orderItemId;
            ProductName = productName;
            Quantity = quantity;
            CorrelationId = correlationId;
        }

        public int OrderId { get; set; }
        public int OrderItemId { get; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string CorrelationId { get; }
    }
}
