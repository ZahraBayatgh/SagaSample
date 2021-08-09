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
    public class ProductController : ControllerBase
    {

        private readonly IEventBus _eventBus;
        private readonly IProductService _productService;

        public ProductController(
            IEventBus eventBus,
            IProductService productService)
        {
            _eventBus = eventBus;
            _productService = productService;

        }


        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductDto productDto)
        {
            try
            {
                var product = new Product
                {
                    Name = productDto.Name,
                    Count = productDto.Count
                };
                var productId = await _productService.AddProductAsync(product);

                return CreatedAtRoute("ProductById", productId);
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}", Name = "ProductById")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _productService.GetProductAsync(id);

                return Ok(product);
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(ProductDto productDto)
        {
            try
            {
                var product = await _productService.UpdateProductAsync(productDto);

                UpdateProductAndAddInventory updateProductIntegrationEvent = new UpdateProductAndAddInventory(product.Name, product.Count, productDto.Count);
                _eventBus.Publish(updateProductIntegrationEvent);

                return NoContent();
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }

    }
}
