using EventBus.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SaleService.Dtos;
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

        [HttpGet("{id}", Name = "ProductById")]
        public async Task<IActionResult> GetProductByIdAsync(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product.IsSuccess)
            {
                return Ok(product);
            }

            return BadRequest(product.Error);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductAsync(CreateProductDto createProductDto)
        {
           
            var productId = await _productService.CreateProductAsync(createProductDto);

            if (productId.IsSuccess)
            {
                return CreatedAtAction("ProductById", productId);
            }

            return BadRequest(productId.Error);
        }

    }
}
