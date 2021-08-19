using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Dtos;
using ProductCatalogService.Services;
using System.Threading.Tasks;

namespace ProductCatalog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductCatalogController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IProductorchestrator _productorchestrator;

        public ProductCatalogController(IProductService productService,
                                        IProductorchestrator productorchestrator)
        {
            _productService = productService;
            _productorchestrator = productorchestrator;
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
            var createProductResponse = await _productorchestrator.CreateProductAndPublishEvent(createProductRequestDto);

            if (createProductResponse.IsSuccess)
            {
                return CreatedAtAction(nameof(GetProductByIdAsync), new { id = createProductResponse.Value }, null);
            }

            return BadRequest(createProductResponse.Error);
        }
    }
}
