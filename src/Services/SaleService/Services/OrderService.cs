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
        /// This metode get order by order id.
        /// If the input id is not valid or an expiration occurs, a Failure will be returned.
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<Result<Order>> GetOrderByIdAsync(int orderId)
        {
            try
            {
                // Check order id
                if (orderId <= 0)
                    return Result.Failure<Order>($"Order id is invalid.");

                // Get order by order id
                var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);

                return Result.Success(order);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get {orderId} order id failed. Exception detail:{ex.Message}");

                return Result.Failure<Order>($"Get {orderId} order id failed.");
            }
        }

        /// <summary>
        /// This method adds a Order to the table.
        /// If the input createProductDto is not valid or an expiration occurs, a Failure will be returned.
        /// </summary>
        /// <param name="createOrderDto"></param>
        public async Task<Result<CreateOrderResponseDto>> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            try
            {
                // Check product instance
                var orderValidation = CheckProductInstance(createOrderDto);
                if (orderValidation.IsFailure)
                    return Result.Failure<CreateOrderResponseDto>(orderValidation.Error);

                // Check product id in database
                var product = await _productService.GetProductByIdAsync(createOrderDto.ProductId);
                if (product.Value == null) 
                return Result.Failure<CreateOrderResponseDto>($"Product Id {createOrderDto.ProductId} is invalid.");

                // Check produt count
                if (product.Value.Count<createOrderDto.Count)
                    return Result.Failure<CreateOrderResponseDto>($"The count of the product {product.Value.Id} id is less than the count of the order");

                // Intialize Order
                Order order = new Order
                {
                    ProductId = createOrderDto.ProductId,
                    Count = createOrderDto.Count
                };

                // Add order in database
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                CreateOrderResponseDto createOrderResponseDto = new CreateOrderResponseDto
                {
                    Name = product.Value.Name,
                    DecreaseCount = createOrderDto.Count
                };
                return Result.Success(createOrderResponseDto);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Add order with {createOrderDto.ProductId} product failed. Exception detail:{ex.Message}");

                return Result.Failure<CreateOrderResponseDto>($"Add order with {createOrderDto.ProductId} product failed.");
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
        /// <param name="createOrderDto"></param>
        /// <returns></returns>
        private Result CheckProductInstance(CreateOrderDto createOrderDto)
        {
            if (createOrderDto==null)
                return Result.Failure($"CreateOrderDto is null.");

            if (createOrderDto.ProductId <= 0)
                return Result.Failure($"ProductId is invalid.");

            if (createOrderDto.Count <= 0)
                return Result.Failure($"Count is invalid.");

            return Result.Success();
        }
    }
}
