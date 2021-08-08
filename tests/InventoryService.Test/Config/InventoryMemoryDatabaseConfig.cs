using InventoryService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryService.Test.Config
{
   public class InventoryMemoryDatabaseConfig
    {
        protected InventoryMemoryDatabaseConfig()
        {
            if (Context != null) return;

            Context = CreateContext();

            new InventoryInitializer().InitializeData(Context);
            new ProductInitializer().InitializeData(Context);
        }

        protected InventoryDbContext Context { get; set; }

        private static InventoryDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<InventoryDbContext>()
              .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
              .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
              .Options;

            return new InventoryDbContext(options);
        }
    }
}
