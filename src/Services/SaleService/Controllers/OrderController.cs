using MediatR;
using Microsoft.AspNetCore.Mvc;
using SaleService.DomainEvents.Events;
using SaleService.Dtos;
using SaleService.Services;
using System.Threading.Tasks;

namespace SaleService.Controllers
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

        [HttpPut]
        public async Task<IActionResult> CreateOrderAsync(CreateOrderDto orderDto)
        {
            // Add order
            var orderResult = await _orderService.CreateOrderAsync(orderDto);

            if (orderResult.IsSuccess)
            {
                // update product count after add order
                var updateProductDomainEvent = new UpdateProductDomainEvent(orderResult.Value.OrderId, orderResult.Value.Name, orderResult.Value.DecreaseCount);
                await _mediator.Publish(updateProductDomainEvent);

                return NoContent();
            }

            return BadRequest(orderResult.Error);
        }

    }
}
