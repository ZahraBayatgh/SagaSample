using InventoryService.Data;
using InventoryService.Models;
using System.Collections.Generic;
using System.Linq;

namespace SagaPattern.UnitTests.Config
{
    public class ProductInitializer
    {
        public void InitializeData(InventoryDbContext dbContext)
        {
            if (!dbContext.Products.Any())
            {
                var products = new List<Product>
                {
                  new Product
                  {
                      Id=1,
                      Name="Mouse",
                  },
                  new Product
                  {
                      Id=2,
                      Name="Monitor",
                  }
                };

                dbContext.Products.AddRange(products);
                dbContext.SaveChanges();
            }
        }
    }
}
