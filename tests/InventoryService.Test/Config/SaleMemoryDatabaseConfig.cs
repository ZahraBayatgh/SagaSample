using InventoryService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SaleService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryService.Test.Config
{
   public class SaleMemoryDatabaseConfig
    {
        protected SaleMemoryDatabaseConfig()
        {
            if (Context != null) return;

            Context = CreateContext();

            new SaleInitializer().InitializeData(Context);
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
