using Microsoft.EntityFrameworkCore;

namespace OrderService.Data
{
    public class OrderDbContext : DbContext
    {
        
        public OrderDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
