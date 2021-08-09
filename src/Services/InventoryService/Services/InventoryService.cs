using InventoryService.Data;
using InventoryService.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InventoryService.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly InventoryDbContext _context;
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(InventoryDbContext context,
            ILogger<InventoryService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<int> AddInventoryAsync(Inventory inventory)
        {
            try
            {
                await _context.Inventories.AddAsync(inventory);

                 await _context.SaveChangesAsync();
                return inventory.Id;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Add {inventory.ProductId} product id in inventory failed. Exception detail:{ex.Message}");

                throw;
            }
        }
    }
}
