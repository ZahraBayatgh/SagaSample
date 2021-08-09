using InventoryService.Models;
using System.Threading.Tasks;

namespace InventoryService.Services
{
    public interface IInventoryService
    {
        Task<int> AddInventoryAsync(Inventory inventory);
    }
}