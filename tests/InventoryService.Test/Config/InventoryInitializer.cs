using InventoryService.Data;
using InventoryService.Models;
using System.Collections.Generic;
using System.Linq;

namespace InventoryService.Test.Config
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
                       ProductId=1,
                       Type=InventoryType.Out,
                       ChangeCount=2,
                       CurrentCount=18
                  },
                   new InventoryTransaction
                  {
                       ProductId=1,
                       Type=InventoryType.Out,
                       ChangeCount=3,
                       CurrentCount=15
                  },
                };

                dbContext.InventoryTransactions.AddRange(inventories);
                dbContext.SaveChanges();
            }
        }
    }
}
