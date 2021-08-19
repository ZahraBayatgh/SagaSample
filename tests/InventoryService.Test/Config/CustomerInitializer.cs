using SalesService.Data;
using SalesService.Models;
using System.Linq;

namespace SagaPattern.UnitTests.Config
{
    public class CustomerInitializer
    {
        public void InitializeData(SaleDbContext dbContext)
        {
            if (!dbContext.Customers.Any())
            {
                var buyer = new Customer
                {
                    Id = 1,
                    FirstName = "Zahra",
                    LastName = "Bayat"
                };

                dbContext.Customers.Add(buyer);
                dbContext.SaveChanges();
            }
        }
    }
}