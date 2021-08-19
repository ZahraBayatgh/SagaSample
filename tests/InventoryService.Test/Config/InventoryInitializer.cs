using InventoryService.Data;
using InventoryService.Models;
using System.Collections.Generic;
using System.Linq;

namespace SagaPattern.UnitTests.Config
{
    public class InventoryInitializer
    {
        public void InitializeData(InventoryDbContext dbContext)
        {
            if (!dbContext.InventoryTransactions.Any())
            {
                var inventories = new List<InventoryTransaction>
                {
                  new InventoryTransaction
                  {
                       Id=1,
                       ProductId=1,
                       InventoryTransactionType=InventoryType.In,
                       Count=2,
                  }
                };

                dbContext.InventoryTransactions.AddRange(inventories);
                dbContext.SaveChanges();
            }
        }
    }
}
