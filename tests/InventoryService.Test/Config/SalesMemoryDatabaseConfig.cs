using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SalesService.Data;
using System;

namespace SagaPattern.UnitTests.Config
{
    public class SalesMemoryDatabaseConfig
    {
        protected SalesMemoryDatabaseConfig()
        {
            if (Context != null) return;

            Context = CreateContext();

            new OrderInitializer().InitializeData(Context);
            new OrderItemInitializer().InitializeData(Context);
            new ProductSalesInitializer().InitializeData(Context);
            new CustomerInitializer().InitializeData(Context);
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
