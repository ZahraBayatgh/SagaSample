using SaleService.Data;
using SaleService.Models;
using System.Linq;

namespace SagaPattern.UnitTests.Config
{
    public class BuyerInitializer
    {
        public void InitializeData(SaleDbContext dbContext)
        {
            if (!dbContext.Buyers.Any())
            {
                var buyer = new Buyer
                {
                    Id = 1,
                    FirstName = "Zahra",
                    LastName = "Bayat"
                };

                dbContext.Buyers.Add(buyer);
                dbContext.SaveChanges();
            }
        }
    }
}