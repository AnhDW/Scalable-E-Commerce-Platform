using Microsoft.EntityFrameworkCore;
using StoreService.Entities;
using System.Diagnostics.CodeAnalysis;

namespace StoreService.Data
{
    public class StoreDbContext : DbContext
    {
        public DbSet<Store> Stores { get; set; }

        public StoreDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
