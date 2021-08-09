using InventoryService.Data;
using InventoryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryService.Test.Config
{
    public class InventoryInitializer
    {
        public void InitializeData(InventoryDbContext dbContext)
        {
            if (!dbContext.Inventories.Any())
            {
                var inventories = new List<Inventory>
                {
                  new Inventory
                  {
                       ProductId=1,
                       Type=Models.InventoryType.Out,
                       Count=2
                  },
                   new Inventory
                  {
                       ProductId=1,
                       Type=Models.InventoryType.Out,
                       Count=3
                  },
                };

                dbContext.Inventories.AddRange(inventories);
                dbContext.SaveChanges();
            }
        }
    }
}
