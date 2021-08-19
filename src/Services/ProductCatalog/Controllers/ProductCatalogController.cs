using EventBus.Abstractions;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Dtos;
using ProductCatalogService.IntegrationEvents.Events;
using ProductCatalogService.Services;
using System.Threading.Tasks;

namespace ProductCatalog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductCatalogController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IEventBus _eventBus;

        public ProductCatalogController(IProductService productService,
                                        IEventBus eventBus)
        {
            _productService = productService;
            _eventBus = eventBus;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductByIdAsync(int id)
        {
            // Get product by product id
            var product = await _productService.GetProductByIdAsync(id);
            if (product.IsSuccess)
            {
                return Ok(product.Value);
            }

            return BadRequest(product.Error);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductAsync(CreateProductRequestDto createProductRequestDto)
        {
            // Create product and inventory transaction
            var createProductResponse = await _productService.CreateProductAsync(createProductRequestDto);

            if (createProductResponse.IsSuccess)
            {
                // Publish CreateProductIntegrationEvent
                CreateProductIntegrationEvent createProductIntegrationEvent = new CreateProductIntegrationEvent(createProductResponse.Value.ProductId,createProductRequestDto.Name, createProductRequestDto.InitialHand, HttpContext.TraceIdentifier);
               await _eventBus.PublishAsync(createProductIntegrationEvent,"test");

                return CreatedAtAction(nameof(GetProductByIdAsync), new { id = createProductResponse.Value.ProductId }, null);
            }

            return BadRequest(createProductResponse.Error);
        }
    }
}
