using SaleService.Data;
using SaleService.Models;
using System.Collections.Generic;
using System.Linq;

namespace InventoryService.Test.Config
{
    public class BuyerInitializer
    {
        public void InitializeData(SaleDbContext dbContext)
        {
            if (!dbContext.Buyers.Any())
            {
                var buyer = new Buyer
                {
                    Id = 1,
                    FirstName = "Zahra",
                    LastName = "Bayat"
                };

                dbContext.Buyers.Add(buyer);
                dbContext.SaveChanges();
            }
        }
    }
}