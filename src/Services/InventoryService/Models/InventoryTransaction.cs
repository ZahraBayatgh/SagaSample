namespace InventoryService.Models
{
    public class InventoryTransaction
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public InventoryType Type { get; set; }
        public int ChangeCount { get; set; }
        public int CurrentCount { get; set; }

    }
    public enum InventoryType
    {
        In = 1,
        Out = 2
    }
}
