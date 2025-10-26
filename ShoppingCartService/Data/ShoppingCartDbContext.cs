using Microsoft.EntityFrameworkCore;
using ShoppingCartService.Entities;

namespace ShoppingCartService.Data
{
    public class ShoppingCartDbContext : DbContext
    {
        public DbSet<CartItem> CartItems { get; set; }
        public ShoppingCartDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CartItem>()
                .HasKey(x => new { x.UserId, x.StoreId, x.ProductVariantId });
        }
    }
}
