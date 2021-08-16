using CSharpFunctionalExtensions;
using CustomerService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SaleService.Data;
using SaleService.Dtos;
using SaleService.Models;
using System;
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
        public async Task<Result<GetOrderResponse>> GetOrderByIdAsync(int orderId)
        {
            try
            {
                // Check order id
                if (orderId <= 0)
                    return Result.Failure<GetOrderResponse>($"Order id is invalid.");

                // Get order by order id
                var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);

                // Check order in db
                if (order==null)
                    return Result.Failure<GetOrderResponse>($"Order is not in db.");
                
                var orderItems =await _context.OrderItems.Where(x => x.OrderId == order.Id).ToListAsync();
                var GetOrderResponse = new GetOrderResponse(order.BuyerId, $"{order.Buyer.FirstName} {order.Buyer.LastName}", order.OrderDate, orderItems);
              
                return Result.Success(GetOrderResponse);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get {orderId} order id failed. Exception detail:{ex.Message}");

                return Result.Failure<GetOrderResponse>($"Get {orderId} order id failed.");
            }
        }
        /// <summary>
        /// This method adds a Order to the table.
        /// If the input createProductDto is not valid or an expiration occurs, a Failure will be returned.
        /// </summary>
        /// <param name="createOrderRequestDto"></param>
        public async Task<Result<int>> CreateOrderAsync(CreateOrderRequestDto createOrderRequestDto)
        {
            try
            {
                // Check product instance
                var orderValidation =await CheckCreateOrderRequestDtoInstanceAsync(createOrderRequestDto);
                if (orderValidation.IsFailure)
                    return Result.Failure<int>(orderValidation.Error);

                // Intialize Order
                Order order = new Order
                {
                    BuyerId = createOrderRequestDto.BuyerId,
                    OrderDate = DateTime.Now
                };

                // Add order in database
                await _context.Orders.AddAsync(order);

                await _context.SaveChangesAsync();

                return Result.Success(order.Id);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Add order with {createOrderRequestDto.BuyerId} buyyer id failed. Exception detail:{ex.Message}");

                return Result.Failure<int>($"Add order with {createOrderRequestDto.BuyerId} buyyer id failed.");
            }
        }
        /// <summary>
        /// This method adds a Order item to the table.
        /// If the input createProductDto is not valid or an expiration occurs, a Failure will be returned.
        /// </summary>
        /// <param name="createOrderItemRequestDto"></param>
        public async Task<Result<CreateOrderItemResponseDto>> CreateOrderItemAsync(CreateOrderItemRequestDto  createOrderItemRequestDto)
        {
            try
            {
                // Check product instance
                var orderValidation =await CheckCreateOrderItemRequestDtoInstanceAsync(createOrderItemRequestDto);
                if (orderValidation.IsFailure)
                    return Result.Failure<CreateOrderItemResponseDto>(orderValidation.Error);

                // Intialize Order
                OrderItem orderItem = new OrderItem
                {
                    OrderId = createOrderItemRequestDto.OrderId,
                    ProductId = createOrderItemRequestDto.ProductId,
                    Quantity = createOrderItemRequestDto.Quantity,
                    UnitPrice = createOrderItemRequestDto.UnitPrice
                };

                // Add order in database
                await _context.OrderItems.AddAsync(orderItem);

             
                await _context.SaveChangesAsync();

                CreateOrderItemResponseDto createOrderResponseDto = new CreateOrderItemResponseDto(orderItem.Id, orderValidation.Value, createOrderItemRequestDto.Quantity);
                return Result.Success(createOrderResponseDto);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Add order with {createOrderItemRequestDto.ProductId} product failed. Exception detail:{ex.Message}");

                return Result.Failure<CreateOrderItemResponseDto>($"Add order with {createOrderItemRequestDto.ProductId} product failed.");
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
        /// This methode check a createOrderRequestDto instance
        /// </summary>
        /// <param name="createOrderRequestDto"></param>
        /// <returns></returns>
        private async Task< Result> CheckCreateOrderRequestDtoInstanceAsync(CreateOrderRequestDto createOrderRequestDto)
        {
            if (createOrderRequestDto == null)
                return Result.Failure($"CreateOrderRequestDto is null.");

            if (createOrderRequestDto.BuyerId <= 0)
                return Result.Failure($"BuyerId is invalid.");

           var buyer=await _context.Buyers.FirstOrDefaultAsync(x=>x.Id==createOrderRequestDto.BuyerId);
            if (buyer==null)
                return Result.Failure($"BuyerId is not in db.");


            return Result.Success();
        }

        /// <summary>
        /// This methode check a CreateOrderItemRequestDto instance
        /// </summary>
        /// <param name="createOrderItemRequestDto"></param>
        /// <returns></returns>
        private async Task< Result<string>> CheckCreateOrderItemRequestDtoInstanceAsync(CreateOrderItemRequestDto createOrderItemRequestDto)
        {
            if (createOrderItemRequestDto == null)
                return Result.Failure<string>($"CreateOrderItemRequestDto is null.");

            if (createOrderItemRequestDto.OrderId <= 0)
                return Result.Failure<string>($"OrderId is invalid.");

            var order = _context.Orders.FirstOrDefaultAsync(x => x.Id == createOrderItemRequestDto.OrderId);
            if (order == null)
                return Result.Failure<string>($"OrderId is not in db.");

            if (createOrderItemRequestDto.ProductId <= 0)
                return Result.Failure<string>($"ProductId is invalid.");

            var product =await _context.Products.FirstOrDefaultAsync(x => x.Id == createOrderItemRequestDto.ProductId);
            if (product == null)
                return Result.Failure<string>($"ProductId is not in db.");

            if (createOrderItemRequestDto.Quantity <= 0)
                return Result.Failure<string>($"Quantity is invalid.");

            if (product.OnHand<createOrderItemRequestDto.Quantity)
                return Result.Failure<string>($"Quantity is more then than product count.");

            if (createOrderItemRequestDto.UnitPrice <= 0)
                return Result.Failure<string>($"UnitPrice is invalid.");

            return Result.Success(product.Name);
        }
    }
}
