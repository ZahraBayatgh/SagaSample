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
        public async Task<Product> GetProductAsync(int id)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get {id} product id failed. Exception detail:{ex.Message}");

                throw;
            }
        }
        public async Task<int> AddProductAsync(Product product)
        {
            try
            {
                await _context.Products.AddAsync(product);

                await _context.SaveChangesAsync();

                return product.Id;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Add {product.Name} product failed. Exception detail:{ex.Message}");
                throw;
            }
        }
        public async Task<Product> UpdateProductAsync(ProductDto productDto)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Name == productDto.Name);

                product.Count = productDto.Count;
                await _context.SaveChangesAsync();

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Update {productDto.Name} product failed. Exception detail:{ex.Message}");
                throw;
            }
        }

    }
}
