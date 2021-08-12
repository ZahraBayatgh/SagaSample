using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SaleService.Data;
using System;

namespace InventoryService.Test.Config
{
    public class SaleMemoryDatabaseConfig
    {
        protected SaleMemoryDatabaseConfig()
        {
            if (Context != null) return;

            Context = CreateContext();

            new OrderInitializer().InitializeData(Context);
            new ProductSaleInitializer().InitializeData(Context);
        }

        protected SaleDbContext Context { get; set; }

        private static SaleDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<SaleDbContext>()
              .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
              .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
              .Options;

            return new SaleDbContext(options);
        }
    }
}
