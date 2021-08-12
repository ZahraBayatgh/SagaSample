namespace InventoryService.Dtos
{
    public class CreateProductResponseDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ChangeCount { get; set; }
        public int CurrentCount { get; set; }
    }
}
