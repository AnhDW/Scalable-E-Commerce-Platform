using BusinessService.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessService.Data
{
    public class BusinessDbContext : DbContext
    {
        public DbSet<Store> Stores { get; set; }
        public DbSet<StoreTag> StoreTags { get; set; }
        public DbSet<StoreCategory> StoreCategories { get; set; }
        public DbSet<StoreTagRelation> StoreTagRelations { get; set; }
        public DbSet<StoreCategoryRelation> StoreCategoryRelations { get; set; }

        public BusinessDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Store>()
                .HasMany(s => s.StoreTagRelations)
                .WithOne(s => s.Store)
                .HasForeignKey(x => x.StoreId);

            modelBuilder.Entity<Store>()
                .HasMany(s => s.StoreCategoryRelations)
                .WithOne(st => st.Store)
                .HasForeignKey(x => x.StoreId);

            modelBuilder.Entity<StoreTag>()
                .HasMany(s => s.StoreTagRelations)
                .WithOne(s => s.StoreTag)
                .HasForeignKey(x => x.StoreTagId);

            modelBuilder.Entity<StoreCategory>()
                .HasMany(s => s.StoreCategoryRelations)
                .WithOne(sc => sc.StoreCategory)
                .HasForeignKey(x => x.StoreCategoryId);

            modelBuilder.Entity<StoreTagRelation>()
                .HasKey(st => new { st.StoreId, st.StoreTagId });

            modelBuilder.Entity<StoreCategoryRelation>()
                .HasKey(sc => new { sc.StoreId, sc.StoreCategoryId });
        }

    }
}
