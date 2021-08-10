using CSharpFunctionalExtensions;
using InventoryService.Dtos;
using InventoryService.Models;
using System.Threading.Tasks;

namespace InventoryService.Services
{
    public interface IInventoryTransactionService
    {
        Task<Result<int>> GetLatestInventoryTransactionByProductIdAsync(int productId);
        Task<Result<InventoryTransaction>> CreateInventoryTransactionAsync(InventoryTransactionDto inventoryTransactionDto);
    }
}