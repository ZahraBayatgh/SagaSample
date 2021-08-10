using CSharpFunctionalExtensions;
using InventoryService.Data;
using InventoryService.Dtos;
using InventoryService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InventoryService.Services
{
    public class ProductService : IProductService
    {
        private readonly InventoryDbContext _context;
        private readonly ILogger<ProductService> _logger;

        public ProductService(InventoryDbContext context,
            ILogger<ProductService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// This metode get product by product id.
        /// If the input id is not valid or an expiration occurs, a Failure will be returned.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<Result<Product>> GetProductByIdAsync(int productId)
        {
            try
            {
                // Check product id
                if (productId <= 0)
                    return Result.Failure<Product>($"Product id is invalid.");

                // Get product by product id
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId);

                return Result.Success(product);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get {productId} product id failed. Exception detail:{ex.Message}");

                return Result.Failure<Product>($"Get {productId} product id failed.");
            }
        }
        public async Task<Result<int>> GetProductIdAsync(string productName)
        {
            try
            {
                // Check product name
                if (string.IsNullOrEmpty(productName))
                    return Result.Failure<int>($"Product name is invalid.");

                // Get product by product name
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Name == productName);
                
                return Result.Success(product.Id);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get {productName} product failed. Exception detail:{ex.Message}");

                return Result.Failure<int>($"Get {productName} product name failed.");
            }
        }

        /// <summary>
        /// This method adds a ProductDto to the table.
        /// If the input createProductDto is not valid or an expiration occurs, a Failure will be returned.
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        public async Task<Result<int>> CreateProductAsync(ProductDto productDto)
        {
            try
            {
                // Check product instance
                var productValidation = CheckCreateProductInstance(productDto);
                if (productValidation.IsFailure)
                    return Result.Failure<int>(productValidation.Error);

                // Intialize product
                var product = new Product
                {
                    Name = productDto.Name,
                    Count = productDto.Count
                };

                // Add product in database
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                return Result.Success(product.Id);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Add {productDto.Name} product failed. Exception detail:{ex.Message}");

                return Result.Failure<int>($"Add {productDto.Name} product failed.");
            }
        }

        /// <summary>
        /// This method update a ProductDto to the table.
        /// If the input productDto is not valid or an expiration occurs, a Failure will be returned.
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        public async Task<Result<Product>> UpdateProductAsync(ProductDto productDto)
        {
            try
            {
                // Check product instance
                var productValidation = CheckUpdateProductInstance(productDto);
                if (productValidation.IsFailure)
                    return Result.Failure<Product>(productValidation.Error);

                // Get product by name
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Name == productDto.Name);

               // Update product
                product.Count = productDto.Count;
                await _context.SaveChangesAsync();

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Update {productDto.Name} product failed. Exception detail:{ex.Message}");
                return Result.Failure<Product>($"Update {productDto.Name} product failed.");

            }
        }

        /// <summary>
        /// This methode check a createProductDto instance
        /// </summary>
        /// <param name="createProductDto"></param>
        /// <returns></returns>
        private static Result CheckCreateProductInstance(ProductDto createProductDto)
        {
            if (createProductDto == null)
                return Result.Failure($"ProductDto instance is invalid.");

            if (string.IsNullOrEmpty(createProductDto.Name))
                return Result.Failure($"Product name is empty.");

            if (createProductDto.Count <= 0)
                return Result.Failure($"Product count is invaild.");

            return Result.Success();
        }

        /// <summary>
        /// This methode check a createProductDto instance
        /// </summary>
        /// <param name="createProductDto"></param>
        /// <returns></returns>
        private static Result CheckUpdateProductInstance(ProductDto createProductDto)
        {
            if (createProductDto == null)
                return Result.Failure($"ProductDto instance is invalid.");

            if (string.IsNullOrEmpty(createProductDto.Name))
                return Result.Failure($"Product name is empty.");

            if (createProductDto.Count < 0)
                return Result.Failure($"Product count is invaild.");

            return Result.Success();
        }
    }
}
