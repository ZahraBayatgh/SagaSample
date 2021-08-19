namespace InventoryService.Dtos
{
    public class CreateProductResponseDto
    {
        public int ProductId { get; set; }

        public CreateProductResponseDto(int productId, string productName = null, int changeCount = 0)
        {
            ProductId = productId;
            ProductName = productName;
            ChangeCount = changeCount;
        }

        public string ProductName { get; set; }
        public int ChangeCount { get; set; }
    }
}
