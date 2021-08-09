using EventBus.Abstractions;
using InventoryService.Dtos;
using InventoryService.IntegrationEvents.Events;
using InventoryService.Models;
using InventoryService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SaleService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {

        private readonly IEventBus _eventBus;
        private readonly IProductService _productService;

        public InventoryController(
            IEventBus eventBus,
            IProductService productService)
        {
            _eventBus = eventBus;
            _productService = productService;

        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductDto productDto)
        {
            _eventBus.Publish(new CancelProductIntegrationEvent("A", 10 + 2));

            await _productService.AddProductAsync(new Product { Name = productDto.Name, Count = productDto.Count });

            return Ok();
        }
    }
}
