using SaleService.Models;
using System;
using System.Collections.Generic;

namespace SaleService.Dtos
{
    public class GetOrderResponse
    {
        public GetOrderResponse(int buyerId, string buyerName, DateTime orderDate, List<OrderItem> orderItems)
        {
            BuyerId = buyerId;
            BuyerName = buyerName;
            OrderDate = orderDate;
            OrderItems = orderItems;
        }

        public int BuyerId { get;private set; }
        public string BuyerName { get; private set; }
        public DateTime OrderDate { get; private set; }
        public List<OrderItem> OrderItems { get; }
    }
}
