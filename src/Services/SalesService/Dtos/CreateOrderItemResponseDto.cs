namespace SalesService.Dtos
{
    public class CreateOrderItemResponseDto
    {

        public CreateOrderItemResponseDto(int orderItemId, string name, int quantity)
        {
            OrderItemId = orderItemId;
            ProductName = name;
            Quantity = quantity;
        }

        public int OrderItemId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
