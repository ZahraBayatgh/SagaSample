using EventBus.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SaleService.Dtos;
using SaleService.IntegrationEvents.Events;
using SaleService.Models;
using SaleService.Services;
using System.Threading.Tasks;

namespace SaleService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SaleController : ControllerBase
    {

        private readonly IEventBus _eventBus;
        private readonly IProductService _productService;

        public SaleController(
            IEventBus eventBus,
            IProductService productService)
        {
            _eventBus = eventBus;
            _productService = productService;

        }

       
        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductDto productDto)
        {
           await _productService.AddProductAsync(new Product { Name = productDto.Name, Count = productDto.Count });

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(ProductDto productDto)
        {
            var result = await _productService.UpdateProductAsync(new ProductDto { Name = productDto.Name, Count = productDto.Count });

            UpdateProductIntegrationEvent updateProductIntegrationEvent = new UpdateProductIntegrationEvent(result.Name, result.Count, productDto.Count);
            _eventBus.Publish(updateProductIntegrationEvent);

            return NoContent();
        }

        [HttpGet()]
        public async Task<IActionResult> GetProduct(int id)
        {
            var result = await _productService.GetProductAsync(id);
          
            return Ok(result);
        }
    }
}
