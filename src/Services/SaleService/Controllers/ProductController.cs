using Microsoft.AspNetCore.Mvc;
using SaleService.Services;
using System.Threading.Tasks;

namespace SaleService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductByIdAsync(int id)
        {
            //Get product by Id
            var product = await _productService.GetProductByIdAsync(id);
            if (product.IsSuccess)
            {
                return Ok(product.Value);
            }

            return BadRequest(product.Error);
        }



    }
}
