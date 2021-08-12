using InventoryService.Dtos;
using InventoryService.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InventoryService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IInventoryOrcasrator _inventoryOrcasrator;
        private readonly IMediator _mediator;

        public ProductController(IProductService productService,
            IInventoryOrcasrator inventoryOrcasrator )
        {
            _productService = productService;
            _inventoryOrcasrator = inventoryOrcasrator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductByIdAsync(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product.IsSuccess)
            {
                return Ok(product.Value);
            }

            return BadRequest(product.Error);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductAsync(ProductDto productDto)
        {
            var createProductResponse = await _inventoryOrcasrator.CreateProductInventoryTransactionAsync(productDto);

            if (createProductResponse.IsSuccess)
            {
                return CreatedAtAction(nameof(GetProductByIdAsync), new { id = createProductResponse.Value }, null);
            }

            return BadRequest(createProductResponse.Error);
        }
    }
}
