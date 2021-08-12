using SaleService.Data;
using SaleService.Models;
using System.Collections.Generic;
using System.Linq;

namespace InventoryService.Test.Config
{
    public class ProductSaleInitializer
    {
        public void InitializeData(SaleDbContext dbContext)
        {
            if (!dbContext.Products.Any())
            {
                var products = new List<Product>
               {
                 new Product
                 {
                     Id=1,
                     Name="Mouse",
                     Count=20
                 },
                 new Product
                 {
                     Id=2,
                     Name="Monitor",
                     Count=12
                 }
            };

                dbContext.Products.AddRange(products);
                dbContext.SaveChanges();
            }
        }
    }
}