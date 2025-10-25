using Microsoft.EntityFrameworkCore;
using ProductCatalogService.Entities;

namespace ProductCatalogService.Data
{
    public class ProductCatalogDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<ProductTagRelation> ProductTagsRelations { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }

        public ProductCatalogDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasOne(x => x.ProductCategory)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.ProductCategoryId);

            modelBuilder.Entity<Product>()
                .HasMany(x=>x.ProductVariants)
                .WithOne(x=>x.Product)
                .HasForeignKey(x=>x.ProductId);

            modelBuilder.Entity<Product>()
                .HasMany(x => x.ProductTagRelations)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId);

            modelBuilder.Entity<Product>()
                .HasMany(x => x.ProductImages)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId);

            modelBuilder.Entity<ProductTag>()
                .HasMany(x => x.ProductTagRelations)
                .WithOne(x => x.ProductTag)
                .HasForeignKey(x => x.ProductTagId);

            modelBuilder.Entity<ProductCategory>()
                .HasMany(x => x.SubCategories)
                .WithOne(x => x.ParentCategory)
                .HasForeignKey(x => x.ParentId);

            modelBuilder.Entity<ProductTagRelation>()
                .HasKey(x => new { x.ProductId, x.ProductTagId });
        }
    }
}
