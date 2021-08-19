namespace InventoryService.Models
{
    public class InventoryTransaction
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        //To do: InventoryTransactionType
        public InventoryType Type { get; set; }
        //To do: Count
        public int ChangeCount { get; set; }
        //To do:Delete
        public int CurrentCount { get; set; }

    }
    public enum InventoryType
    {
        In = 1,
        Out = 2
    }
}
