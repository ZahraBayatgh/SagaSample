using SalesService.Data;
using SalesService.Models;
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
                       CustomerId=1,
                       OrderDate=DateTime.Now,
                   },
                 };

                dbContext.Orders.AddRange(orders);
                dbContext.SaveChanges();
            }
        }
    }
}