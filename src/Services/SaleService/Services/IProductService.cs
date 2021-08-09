using SaleService.Dtos;
using SaleService.Models;
using System.Threading.Tasks;

namespace SaleService.Services
{
    public interface IProductService
    {
        Task<Product> GetProductAsync(int id);
        Task<int> AddProductAsync(Product product);
        Task<Product> UpdateProductAsync(ProductDto product);
        Task<bool> CancelProductAsync(ProductDto product);
    }
}