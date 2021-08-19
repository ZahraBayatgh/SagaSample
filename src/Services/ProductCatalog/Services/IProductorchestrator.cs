using CSharpFunctionalExtensions;
using ProductCatalog.Dtos;
using System.Threading.Tasks;

namespace ProductCatalogService.Services
{
    public interface IProductorchestrator
    {
        Task<Result<int>> CreateProductAndPublishEvent(CreateProductRequestDto createProductRequestDto);
    }
}