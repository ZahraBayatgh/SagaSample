using InventoryService.Dtos;
using InventoryService.Models;
using InventoryService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InventoryService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
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
        public async Task<IActionResult> CreateProductAsync(ProductDto productDto)
        {

            var productId = await _productService.CreateProductAsync(productDto);

            if (productId.IsSuccess)
            {
                return CreatedAtRoute("ProductById", productId);
            }

            return BadRequest(productId.Error);
        }
    }
}
