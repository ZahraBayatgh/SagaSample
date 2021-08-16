using CSharpFunctionalExtensions;
using ProductCatalog.Dtos;
using ProductCatalogService.Models;
using SaleService.Dtos;
using System.Threading.Tasks;

namespace ProductCatalogService.Services
{
    public interface IProductService
    {
        Task<Result<CreateProductResponseDto>> CreateProductAsync(CreateProductRequestDto createProductRequestDto);
        Task<Result> DeleteProductAsync(int productId);
        Task<Result<Product>> GetProductByIdAsync(int productId);
        Task<Result> UpdateProductStatusAsync(UpdateProductStatusRequestDto updateProductStatusRequestDto);
    }
}