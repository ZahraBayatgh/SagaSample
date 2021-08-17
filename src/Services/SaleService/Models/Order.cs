using System;
using System.Collections;
using System.Collections.Generic;

namespace SaleService.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int BuyerId { get; set; }
        public DateTime OrderDate { get; set; }

    }
}
