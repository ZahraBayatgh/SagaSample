using System;
using System.Collections;
using System.Collections.Generic;

namespace SalesService.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }

    }
}
