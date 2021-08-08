using Microsoft.EntityFrameworkCore;
using SaleService.Data;
using SaleService.Dtos;
using SaleService.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SaleService.Services
{
    public class ProductService : IProductService
    {
        private readonly SaleDbContext _context;

        public ProductService(SaleDbContext context)
        {
            _context = context;
        }
        public async Task<Product> UpdateProductAsync(ProductDto product)
        {
            var result =await _context.Products.FirstOrDefaultAsync(x => x.Name == product.Name);

            result.Count -= product.Count;
            await _context.SaveChangesAsync();

            return result;
        }
        public async Task<bool> CancelProductAsync(ProductDto product)
        {
            try
            {
                var result = await _context.Products.FirstOrDefaultAsync(x => x.Name == product.Name);

                result.Count = product.Count;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (System.Exception)
            {

                throw;
            }
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

        public async Task<Product> GetProductAsync(int id)
        {
            var result =await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }
    }
}
