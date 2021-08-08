using InventoryService.Data;
using InventoryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly InventoryDbContext _context;

        public InventoryService(InventoryDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddInventoryAsync(Inventory inventory)
        {
            try
            {
                await _context.Inventories.AddAsync(inventory);

                 await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
