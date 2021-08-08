using Microsoft.EntityFrameworkCore;
using SaleService.Models;

namespace SaleService.Data
{
    public class SaleDbContext : DbContext
    {
        public SaleDbContext(DbContextOptions<SaleDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
    }
}
