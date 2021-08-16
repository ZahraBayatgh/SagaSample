using SaleService.Data;
using SaleService.Models;
using System.Collections.Generic;
using System.Linq;

namespace SagaPattern.UnitTests.Config
{
    public class ProductSalesInitializer
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
                     OnHand=20
                 },
                 new Product
                 {
                     Id=2,
                     Name="Monitor",
                     OnHand=12
                 }
            };

                dbContext.Products.AddRange(products);
                dbContext.SaveChanges();
            }
        }
    }
}