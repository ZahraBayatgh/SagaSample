using SaleService.Models;
using System;
using System.Collections.Generic;

namespace SaleService.Dtos
{
    public class CreateOrderRequestDto
    {
        public CreateOrderRequestDto(int buyerId)
        {
            BuyerId = buyerId;
        }

        public int BuyerId { get; private set; }
    }
}
