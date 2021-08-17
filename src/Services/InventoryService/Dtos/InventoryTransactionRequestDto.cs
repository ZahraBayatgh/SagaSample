using InventoryService.Models;

namespace InventoryService.Dtos
{
    public class InventoryTransactionRequestDto
    {

        public InventoryTransactionRequestDto(int productId, int changeCount, int currentCount , InventoryType type )
        {
            ProductId = productId;
            ChangeCount = changeCount;
            CurrentCount = currentCount;
            Type = type;
        }
        public int ProductId { get;private set; }
        public int ChangeCount { get; private set; }
        public int CurrentCount { get; private set; }
        public InventoryType Type { get; private set; }
    }
}
