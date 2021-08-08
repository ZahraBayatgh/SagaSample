using SaleService.Dtos;
using SaleService.Models;
using System.Threading.Tasks;

namespace SaleService.Services
{
    public interface IProductService
    {
        Task<bool> AddProductAsync(Product product);
        Task<Product> GetProductAsync(int id);
        Task<Product> UpdateProductAsync(ProductDto product);
        Task<bool> CancelProductAsync(ProductDto product);
    }
}