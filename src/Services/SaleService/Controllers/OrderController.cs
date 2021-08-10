using EventBus.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SaleService.DomainEvents.Events;
using SaleService.Dtos;
using SaleService.IntegrationEvents.Events;
using SaleService.Services;
using System.Threading.Tasks;

namespace SaleService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {

        private readonly IEventBus _eventBus;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IMediator _mediator;

        public OrderController(
            IEventBus eventBus,
            IOrderService orderService,
            IProductService productService,
            IMediator mediator
            )
        {
            _eventBus = eventBus;
            _orderService = orderService;
            _productService = productService;
            _mediator = mediator;
        }

        [HttpPut]
        public async Task<IActionResult> CreateOrderAsync(OrderDto orderDto)
        {
           var orderResult= await _orderService.CreateOrderAsync(orderDto);

            if (orderResult.IsSuccess)
            {
                var updateProductDomainEvent = new UpdateProductDomainEvent(orderResult.Value.OrderId,orderResult.Value.Name, orderResult.Value.DecreaseCount);
                await _mediator.Send(updateProductDomainEvent);

                UpdateProductCountAndAddInventoryTransaction updateProductIntegrationEvent = new UpdateProductCountAndAddInventoryTransaction(orderResult.Value.Name, orderResult.Value.DecreaseCount);
                _eventBus.Publish(updateProductIntegrationEvent);

                return NoContent();
            }

            return BadRequest(orderResult.Error);
        }

    }
}
