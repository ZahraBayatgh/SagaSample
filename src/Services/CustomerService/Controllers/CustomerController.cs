using CustomerService.Dtos;
using CustomerService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private IBuyerService _buyerService;

        public CustomerController(IBuyerService buyerService)
        {
            _buyerService = buyerService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerByIdAsync(int id)
        {
            // Get customer by customer id
            var customer = await _buyerService.GetCustomerByIdAsync(id);
            if (customer.IsSuccess)
            {
                return Ok(customer.Value);
            }

            return BadRequest(customer.Error);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CustomerDto customerDto)
        {
            // Create customer and inventory transaction
            var createCustomerResponse = await _buyerService.CreateCustomerAsync(customerDto);

            if (createCustomerResponse.IsSuccess)
            {
                return CreatedAtAction(nameof(GetCustomerByIdAsync), new { id = createCustomerResponse.Value }, null);
            }

            return BadRequest(createCustomerResponse.Error);
        }
    }
}
