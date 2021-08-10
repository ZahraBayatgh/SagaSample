using CSharpFunctionalExtensions;
using InventoryService.Dtos;
using InventoryService.Models;
using System.Threading.Tasks;

namespace InventoryService.Services
{
    public interface IProductService
    {
        Task<Result<Product>> GetProductByIdAsync(int productId);
        Task<Result<int>> GetProductIdAsync(string name);
        Task<Result<int>> CreateProductAsync(ProductDto productDto);
        Task<Result<Product>> UpdateProductAsync(ProductDto product);
    }
}