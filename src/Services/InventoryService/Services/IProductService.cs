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
        Task<Result<CreateProductResponseDto>> CreateProductAsync(ProductDto productDto);
        Task<Result> DeleteProductAsync(int productId);
        Task<Result<Product>> UpdateProductAsync(ProductDto product);
    }
}