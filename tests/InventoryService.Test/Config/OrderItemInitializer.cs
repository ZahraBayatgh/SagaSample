using SalesService.Data;
using SalesService.Models;
using System.Collections.Generic;
using System.Linq;

namespace SagaPattern.UnitTests.Config
{
    public class OrderItemInitializer
    {
        public void InitializeData(SaleDbContext dbContext)
        {
            if (!dbContext.OrderItems.Any())
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
                   },
                   new OrderItem
                   {
                       Id=2,
                       OrderId=1,
                       ProductId=2,
                       Quantity=10,
                       UnitPrice=100
                   },
                 };

                dbContext.OrderItems.AddRange(orderItems);
                dbContext.SaveChanges();
            }
        }
    }
}