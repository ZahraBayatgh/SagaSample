using Microsoft.AspNetCore.Mvc;
using SalesService.Dtos;
using SalesService.Services;
using System.Threading.Tasks;

namespace SalesService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerByIdAsync(int id)
        {
            // Get customer by customer id
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer.IsSuccess)
            {
                return Ok(customer.Value);
            }

            return BadRequest(customer.Error);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CreateCustomerRequestDto createCustomerRequest)
        {
            // Create customer and inventory transaction
            var createCustomerResponse = await _customerService.CreateCustomerAsync(createCustomerRequest);

            if (createCustomerResponse.IsSuccess)
            {
                return CreatedAtAction(nameof(GetCustomerByIdAsync), new { id = createCustomerResponse.Value }, null);
            }

            return BadRequest(createCustomerResponse.Error);
        }
    }
}
