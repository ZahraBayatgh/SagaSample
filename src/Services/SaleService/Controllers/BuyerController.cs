using CustomerService.Services;
using Microsoft.AspNetCore.Mvc;
using SaleService.Dtos;
using System.Threading.Tasks;

namespace CustomerService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BuyerController : ControllerBase
    {
        private IBuyerService _buyerService;

        public BuyerController(IBuyerService buyerService)
        {
            _buyerService = buyerService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBuyerByIdAsync(int id)
        {
            // Get buyer by buyer id
            var buyer = await _buyerService.GetBuyerByIdAsync(id);
            if (buyer.IsSuccess)
            {
                return Ok(buyer.Value);
            }

            return BadRequest(buyer.Error);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBuyerAsync(CreateBuyerRequestDto createBuyerRequest)
        {
            // Create buyer and inventory transaction
            var createBuyerResponse = await _buyerService.CreateBuyerAsync(createBuyerRequest);

            if (createBuyerResponse.IsSuccess)
            {
                return CreatedAtAction(nameof(GetBuyerByIdAsync), new { id = createBuyerResponse.Value }, null);
            }

            return BadRequest(createBuyerResponse.Error);
        }
    }
}
