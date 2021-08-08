using InventoryService.Dtos;
using InventoryService.Models;
using System.Threading.Tasks;

namespace InventoryService.Services
{
    public interface IProductService
    {
        Task<bool> AddProductAsync(Product product);
        Task<Product> UpdateProductAsync(ProductDto product);
    }
}