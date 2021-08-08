namespace InventoryService.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Type Type { get; set; }
        public int Count { get; set; }

    }
    public enum Type
    {
        In=1,
        Out=2
    }
}
