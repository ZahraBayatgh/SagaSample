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

                // Check product in db
                if (product == null)
                    return Result.Failure<Product>($"Product not found.");

                return Result.Success(product);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get {productId} product id failed. Exception detail:{ex.Message}");

                return Result.Failure<Product>($"Get {productId} product id failed.");
            }
        }

        /// <summary>
        /// This metode get product by product name.
        /// If the input name is not valid or an expiration occurs, a Failure will be returned.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<Result<Product>> GetProductByNameAsync(string productName)
        {
            try
            {
                // Check product name
                if (string.IsNullOrEmpty(productName))
                    return Result.Failure<Product>($"Product name is null.");

                // Get product by product name
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Name == productName);

                // Check product in db
                if (product == null)
                    return Result.Failure<Product>($"Product not found.");

                return Result.Success(product);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get {productName} product name failed. Exception detail:{ex.Message}");

                return Result.Failure<Product>($"Get {productName} product name failed.");
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

                // Check product in db
                if (product == null)
                    return Result.Failure<int>($"Product not found.");

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
        public async Task<Result<CreateProductResponseDto>> CreateProductAsync(ProductRequestDto productDto)
        {
            try
            {
                // Check product instance
                var productValidation = CheckCreateProductInstance(productDto);
                if (productValidation.IsFailure)
                    return Result.Failure<CreateProductResponseDto>(productValidation.Error);

                // Intialize product
                var product = new Product
                {
                    Name = productDto.ProductName,
                };

                // Add product in database
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                //Intialize CreateProductResponseDto
                CreateProductResponseDto createProductResponseDto = new CreateProductResponseDto(product.Id, product.Name, productDto.Count);

                return Result.Success(createProductResponseDto);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Add {productDto.ProductName} product failed. Exception detail:{ex.Message}");

                return Result.Failure<CreateProductResponseDto>($"Add {productDto.ProductName} product failed.");
            }
        }

        /// <summary>
        /// This method delete a Product to the table.
        /// If the input productId is not valid or an expiration occurs, a Failure will be returned.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<Result> DeleteProductAsync(int productId)
        {
            try
            {
                // Check product id
                if (productId <= 0)
                    return Result.Failure($"Product id is zero.");

                // Get product by product id
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId);
                if (product == null)
                    return Result.Failure($"Product id is invalid.");

                // Remove product
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Delete product with {productId} id failed. Exception detail:{ex.Message}");

                return Result.Failure($"Delete product with {productId} id failed.");
            }
        }
        /// <summary>
        /// This methode check a createProductDto instance
        /// </summary>
        /// <param name="createProductDto"></param>
        /// <returns></returns>
        private static Result CheckCreateProductInstance(ProductRequestDto createProductDto)
        {
            if (createProductDto == null)
                return Result.Failure($"ProductDto instance is invalid.");

            if (string.IsNullOrEmpty(createProductDto.ProductName))
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
        private static Result CheckUpdateProductInstance(ProductRequestDto createProductDto)
        {
            if (createProductDto == null)
                return Result.Failure($"ProductDto instance is invalid.");

            if (string.IsNullOrEmpty(createProductDto.ProductName))
                return Result.Failure($"Product name is empty.");

            if (createProductDto.Count < 0)
                return Result.Failure($"Product count is invaild.");

            return Result.Success();
        }
    }
}
