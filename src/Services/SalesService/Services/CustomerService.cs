using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SalesService.Data;
using SalesService.Dtos;
using SalesService.Models;
using System;
using System.Threading.Tasks;

namespace SalesService.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly SaleDbContext _context;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(SaleDbContext context, ILogger<CustomerService> logger)
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
                if (customer == null)
                    return Result.Failure<Customer>($"Customer id is invalid.");

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
        /// <param name="createCustomerRequest"></param>
        /// <returns></returns>
        public async Task<Result<int>> CreateCustomerAsync(CreateCustomerRequestDto createCustomerRequest)
        {
            try
            {
                // Check customer instance
                var customerValidation = CheckCreateCustomerInstance(createCustomerRequest);
                if (customerValidation.IsFailure)
                    return Result.Failure<int>(customerValidation.Error);

                // Intialize customer
                var customer = new Customer
                {
                    FirstName = createCustomerRequest.FirstName,
                    LastName = createCustomerRequest.LastName
                };

                // Add customer in database
                await _context.Customers.AddAsync(customer);
                await _context.SaveChangesAsync();

                return Result.Success(customer.Id);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Add {createCustomerRequest.FirstName} {createCustomerRequest.LastName} customer failed. Exception detail:{ex.Message}");

                return Result.Failure<int>($"Add {createCustomerRequest.FirstName} {createCustomerRequest.LastName} customer failed.");
            }
        }

        /// <summary>
        /// This methode check a customerDto instance
        /// </summary>
        /// <param name="createProductDto"></param>
        /// <returns></returns>
        private Result CheckCreateCustomerInstance(CreateCustomerRequestDto createCustomerRequest)
        {
            if (createCustomerRequest == null)
                return Result.Failure($"CustomerDto is null.");

            if (string.IsNullOrEmpty(createCustomerRequest.FirstName))
                return Result.Failure($"FirstName is empty.");

            if (string.IsNullOrEmpty(createCustomerRequest.LastName))
                return Result.Failure($"LastName is empty.");

            return Result.Success();
        }
    }
}
