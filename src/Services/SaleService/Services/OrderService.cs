using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SaleService.Data;
using SaleService.Dtos;
using SaleService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaleService.Services
{
    public class OrderService : IOrderService
    {
        private readonly SaleDbContext _context;
        private readonly ILogger<OrderService> _logger;
        private readonly IProductService _productService;

        public OrderService(SaleDbContext context,
            ILogger<OrderService> logger,
            IProductService productService)
        {
            _context = context;
            _logger = logger;
            _productService = productService;
        }

        /// <summary>
        /// This method adds a Order to the table.
        /// If the input createProductDto is not valid or an expiration occurs, a Failure will be returned.
        /// </summary>
        /// <param name="orderDto"></param>
        public async Task<Result<CreateOrderDto>> CreateOrderAsync(OrderDto orderDto)
        {
            try
            {
                // Check product instance
                var orderValidation = CheckProductInstance(orderDto);
                if (orderValidation.IsFailure)
                    return Result.Failure<CreateOrderDto>(orderValidation.Error);

                // Check product id in database
                var product = await _productService.GetProductByIdAsync(orderDto.ProductId);
                if (product.Value == null) 
                return Result.Failure<CreateOrderDto>($"Product Id {orderDto.ProductId} is invalid.");

                // Intialize Order
                Order order = new Order
                {
                    ProductId = orderDto.ProductId,
                    Count = orderDto.Count
                };

                // Add order in database
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                CreateOrderDto createOrderDto = new CreateOrderDto
                {
                    Name = product.Value.Name,
                    DecreaseCount = orderDto.Count
                };
                return Result.Success(createOrderDto);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Add order with {orderDto.ProductId} product failed. Exception detail:{ex.Message}");

                return Result.Failure<CreateOrderDto>($"Add order with {orderDto.ProductId} product failed.");
            }
        }

        /// <summary>
        /// This method delete a Order to the table.
        /// If the input orderId is not valid or an expiration occurs, a Failure will be returned.
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<Result> DeleteOrderAsync(int orderId)
        {
            try
            {
                // Check product id
                if (orderId <= 0)
                    return Result.Failure($"Order id is zero.");

                // Get order by product id
                var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
                if (order == null)
                    return Result.Failure($"Order id is invalid.");

                // Remove order
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Delete order with {orderId} id failed. Exception detail:{ex.Message}");

                return Result.Failure($"Delete order with {orderId} id failed.");
            }
        }
        /// <summary>
        /// This methode check a orderDto instance
        /// </summary>
        /// <param name="orderDto"></param>
        /// <returns></returns>
        private Result CheckProductInstance(OrderDto orderDto)
        {
            if (orderDto.ProductId <= 0)
                return Result.Failure($"ProductId is invalid.");

            if (orderDto.Count <= 0)
                return Result.Failure($"Count is invalid.");

            return Result.Success();
        }
    }
}
