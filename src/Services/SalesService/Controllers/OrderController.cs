using MediatR;
using Microsoft.AspNetCore.Mvc;
using SalesService.DomainEvents.Events;
using SalesService.Dtos;
using SalesService.Services;
using System.Threading.Tasks;

namespace SalesService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {

        private readonly IOrderService _orderService;
        private readonly IMediator _mediator;

        public OrderController(IOrderService orderService,
                               IMediator mediator)
        {
            _orderService = orderService;
            _mediator = mediator;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderByIdAsync(int id)
        {
            //Get order by Id
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order.IsSuccess)
            {
                return Ok(order.Value);
            }

            return BadRequest(order.Error);
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync(CreateOrderRequestDto createOrderRequestDto)
        {
            // Add order
            var orderResult = await _orderService.CreateOrderAsync(createOrderRequestDto);

            if (orderResult.IsSuccess)
            {
                return CreatedAtAction(nameof(GetOrderByIdAsync), new { id = orderResult.Value }, null);

            }

            return BadRequest(orderResult.Error);
        }

        [HttpPut]
        public async Task<IActionResult> CreateOrderItemAsync(CreateOrderItemRequestDto createOrderItemRequestDto)
        {
            // Add order
            var orderResult = await _orderService.CreateOrderItemAsync(createOrderItemRequestDto);

            if (orderResult.IsSuccess)
            {
                // update product count after add order
                var updateProductDomainEvent = new UpdateProductDomainEvent(orderResult.Value.OrderItemId, createOrderItemRequestDto.OrderId, orderResult.Value.ProductName, orderResult.Value.Quantity, HttpContext.TraceIdentifier);
                await _mediator.Publish(updateProductDomainEvent);

                return NoContent();
            }

            return BadRequest(orderResult.Error);
        }

    }
}
