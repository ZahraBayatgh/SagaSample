using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SaleService.Data;
using SaleService.Dtos;
using SaleService.Models;
using System;
using System.Threading.Tasks;

namespace CustomerService.Services
{
    public class BuyerService : IBuyerService
    {
        private readonly SaleDbContext _context;
        private readonly ILogger<BuyerService> _logger;

        public BuyerService(SaleDbContext context, ILogger<BuyerService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// This metode get buyer by buyer id.
        /// If the input id is not valid or an expiration occurs, a Failure will be returned.
        /// </summary>
        /// <param name="buyerId"></param>
        /// <returns></returns>
        public async Task<Result<Buyer>> GetBuyerByIdAsync(int buyerId)
        {
            try
            {
                // Check buyer id
                if (buyerId <= 0)
                    return Result.Failure<Buyer>($"Buyer id is invalid.");

                // Get buyer by buyer id
                var buyer = await _context.Buyers.FirstOrDefaultAsync(x => x.Id == buyerId);

                return Result.Success(buyer);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get {buyerId} buyer id failed. Exception detail:{ex.Message}");

                return Result.Failure<Buyer>($"Get {buyerId} buyer id failed.");
            }
        }

        /// <summary>
        /// This method adds a Buyer to the table.
        /// If the input createBuyerDto is not valid or an expiration occurs, a Failure will be returned.
        /// </summary>
        /// <param name="buyerDto"></param>
        /// <returns></returns>
        public async Task<Result<int>> CreateBuyerAsync(BuyerDto buyerDto)
        {
            try
            {
                // Check buyer instance
                var buyerValidation = CheckCreateBuyerInstance(buyerDto);
                if (buyerValidation.IsFailure)
                    return Result.Failure<int>(buyerValidation.Error);

                // Intialize buyer
                var buyer = new Buyer
                {
                    FirstName = buyerDto.FirstName,
                    LastName = buyerDto.LastName
                };

                // Add buyer in database
                await _context.Buyers.AddAsync(buyer);
                await _context.SaveChangesAsync();

                return Result.Success(buyer.Id);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Add {buyerDto.FirstName} {buyerDto.LastName} buyer failed. Exception detail:{ex.Message}");

                return Result.Failure<int>($"Add {buyerDto.FirstName} {buyerDto.LastName} buyer failed.");
            }
        }

        /// <summary>
        /// This methode check a buyerDto instance
        /// </summary>
        /// <param name="createProductDto"></param>
        /// <returns></returns>
        private Result CheckCreateBuyerInstance(BuyerDto buyerDto)
        {
            if (buyerDto==null)
                return Result.Failure($"BuyerDto is null.");

            if (string.IsNullOrEmpty(buyerDto.FirstName))
                return Result.Failure($"FirstName is empty.");

            if (string.IsNullOrEmpty(buyerDto.LastName))
                return Result.Failure($"LastName is empty.");

            return Result.Success();
        }
    }
}
