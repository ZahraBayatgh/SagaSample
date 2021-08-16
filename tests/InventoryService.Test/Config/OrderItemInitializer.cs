using SaleService.Data;
using SaleService.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SagaPattern.UnitTests.Config
{
    public class OrderItemInitializer
    {
        public void InitializeData(SaleDbContext dbContext)
        {
            if (!dbContext.Orders.Any())
            {
                var orderItems = new List<OrderItem>
                 {
                   new OrderItem
                   {
                       Id=1,
                       OrderId=1,
                       ProductId=1,
                       Quantity=10,
                       UnitPrice=100
                   }
                 };

                dbContext.OrderItems.AddRange(orderItems);
                dbContext.SaveChanges();
            }
        }
    }
}