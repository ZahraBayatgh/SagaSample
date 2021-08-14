using InventoryService.Models;

namespace InventoryService.Dtos
{
    public class InventoryTransactionDto
    {
        public int ProductId { get; set; }
        public int ChangeCount { get; set; }
        public int CurrentCount { get; set; }
        public InventoryType Type { get; set; }
    }
}
