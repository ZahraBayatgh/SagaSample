using SaleService.Data;
using SaleService.Models;
using System.Collections.Generic;
using System.Linq;

namespace InventoryService.Test.Config
{
    public class OrderInitializer
    {
        public void InitializeData(SaleDbContext dbContext)
        {
            if (!dbContext.Orders.Any())
            {
                var orders = new List<Order>
                 {
                   new Order
                   {
                       Id=1,
                       ProductId=1,
                        Count=2
                   },
                 };

                dbContext.Orders.AddRange(orders);
                dbContext.SaveChanges();
            }
        }
    }
}