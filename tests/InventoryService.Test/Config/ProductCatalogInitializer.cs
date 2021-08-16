using ProductCatalog.Data;
using ProductCatalogService.Models;
using System.Collections.Generic;
using System.Linq;

namespace SagaPattern.UnitTests.Config
{
    public class ProductCatalogInitializer
    {
        public void InitializeData(ProductCatalogDbContext dbContext)
        {
            if (!dbContext.Products.Any())
            {
                var products = new List<Product>
                {
                  new Product
                  {
                      Id=1,
                      Name="Mouse",
                      ProductStatus=ProductStatus.Completed
                  },
                  new Product
                  {
                      Id=2,
                      Name="Monitor",
                      ProductStatus=ProductStatus.SalesIsOk
                  },
                   new Product
                  {
                      Id=3,
                      Name="Keyboard",
                      ProductStatus=ProductStatus.InventoryIsOk
                  }
                };

                dbContext.Products.AddRange(products);
                dbContext.SaveChanges();
            }
        }
    }
}
