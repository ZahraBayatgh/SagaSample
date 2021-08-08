using InventoryService.Data;
using InventoryService.Dtos;
using InventoryService.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace InventoryService.Services
{
    public class ProductService : IProductService
    {
        private readonly InventoryDbContext _context;

        public ProductService(InventoryDbContext context)
        {
            _context = context;
        }
        public async Task<Product> UpdateProductAsync(ProductDto product)
        {
            var result =await _context.Products.FirstOrDefaultAsync(x => x.Name == product.Name);

            result.Count = product.Count;
            await _context.SaveChangesAsync();

            return result;
        }
        public async Task<bool> AddProductAsync(Product product)
        {
            try
            {
                await _context.Products.AddAsync(product);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}
