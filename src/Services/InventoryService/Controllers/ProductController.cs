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
    }
}
