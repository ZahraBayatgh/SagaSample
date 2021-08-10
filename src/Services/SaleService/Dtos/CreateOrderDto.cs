namespace SaleService.Dtos
{
    public class CreateOrderDto
    {
        public int OrderId { get; set; }
        public string Name { get; set; }
        public int DecreaseCount { get; set; }
    }
}
