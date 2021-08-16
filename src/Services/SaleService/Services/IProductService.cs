using CSharpFunctionalExtensions;
using SaleService.Dtos;
using SaleService.Models;
using System.Threading.Tasks;

namespace SaleService.Services
{
    public interface IProductService
    {
        Task<Result<Product>> GetProductByIdAsync(int productId);
        Task<Result<Product>> GetProductByNameAsync(string productName);
        Task<Result<int>> CreateProductAsync(CreateProductRequestDto createProductRequestDto);
        Task<Result> DeleteProductAsync(int productId);
        Task<Result> UpdateProductCountAsync(UpdateProductCountDto updateProductDto);
        Task<Result> CancelChangeProductCountAsync(CancelChangeProductCountDto createProductDto);
    }
}