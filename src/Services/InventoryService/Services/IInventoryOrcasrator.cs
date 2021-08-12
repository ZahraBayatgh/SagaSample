using CSharpFunctionalExtensions;
using InventoryService.Dtos;
using System.Threading.Tasks;

namespace InventoryService.Services
{
    public interface IInventoryOrcasrator
    {
        Task<Result<int>> CreateProductInventoryTransactionAsync(ProductDto productDto);
    }
}