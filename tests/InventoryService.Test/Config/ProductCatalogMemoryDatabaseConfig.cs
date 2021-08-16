using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProductCatalog.Data;
using System;

namespace SagaPattern.UnitTests.Config
{
    public class ProductCatalogMemoryDatabaseConfig
    {
        protected ProductCatalogMemoryDatabaseConfig()
        {
            if (Context != null) return;

            Context = CreateContext();

            new ProductCatalogInitializer().InitializeData(Context);
        }

        protected ProductCatalogDbContext Context { get; set; }

        private static ProductCatalogDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ProductCatalogDbContext>()
              .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
              .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
              .Options;

            return new ProductCatalogDbContext(options);
        }
    }
}
