using CSharpFunctionalExtensions;
using CustomerService.Data;
using CustomerService.Dtos;
using CustomerService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CustomerService.Services
{
    public class BuyerService : IBuyerService
    {
        private readonly CustomerDbContext _context;
        private readonly ILogger<BuyerService> _logger;

        public BuyerService(CustomerDbContext context, ILogger<BuyerService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// This metode get customer by customer id.
        /// If the input id is not valid or an expiration occurs, a Failure will be returned.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<Result<Customer>> GetCustomerByIdAsync(int customerId)
        {
            try
            {
                // Check customer id
                if (customerId <= 0)
                    return Result.Failure<Customer>($"Customer id is invalid.");

                // Get customer by customer id
                var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == customerId);

                return Result.Success(customer);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get {customerId} customer id failed. Exception detail:{ex.Message}");

                return Result.Failure<Customer>($"Get {customerId} customer id failed.");
            }
        }

        /// <summary>
        /// This method adds a Customer to the table.
        /// If the input createCustomerDto is not valid or an expiration occurs, a Failure will be returned.
        /// </summary>
        /// <param name="customerDto"></param>
        /// <returns></returns>
        public async Task<Result<int>> CreateCustomerAsync(CustomerDto customerDto)
        {
            try
            {
                // Check customer instance
                var customerValidation = CheckCreateCustomerInstance(customerDto);
                if (customerValidation.IsFailure)
                    return Result.Failure<int>(customerValidation.Error);

                // Intialize customer
                var customer = new Customer
                {
                    FirstName = customerDto.FirstName,
                    LastName = customerDto.LastName
                };

                // Add customer in database
                await _context.Customers.AddAsync(customer);
                await _context.SaveChangesAsync();

                return Result.Success(customer.Id);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Add {customerDto.FirstName} {customerDto.LastName} customer failed. Exception detail:{ex.Message}");

                return Result.Failure<int>($"Add {customerDto.FirstName} {customerDto.LastName} customer failed.");
            }
        }

        /// <summary>
        /// This methode check a customerDto instance
        /// </summary>
        /// <param name="createProductDto"></param>
        /// <returns></returns>
        private Result CheckCreateCustomerInstance(CustomerDto customerDto)
        {

            if (string.IsNullOrEmpty(customerDto.FirstName))
                return Result.Failure($"FirstName is empty.");

            if (string.IsNullOrEmpty(customerDto.LastName))
                return Result.Failure($"LastName is empty.");

            return Result.Success();
        }
    }
}
