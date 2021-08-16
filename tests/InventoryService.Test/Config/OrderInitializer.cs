using SaleService.Data;
using SaleService.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SagaPattern.UnitTests.Config
{
    public class OrderInitializer
    {
        public void InitializeData(SaleDbContext dbContext)
        {
            if (!dbContext.Orders.Any())
            {
                var orders = new List<Order>
                 {
                   new Order
                   {
                       Id=1,
                       BuyerId=1,
                        Buyer=new Buyer
                        {
                             Id=1,
                              FirstName="Zahra",
                              LastName="Bayat"
                        },
                         OrderDate=DateTime.Now,
                   },
                 };

                dbContext.Orders.AddRange(orders);
                dbContext.SaveChanges();
            }
        }
    }
}