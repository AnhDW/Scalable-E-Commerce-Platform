namespace ProductCatalogService.Entities
{
    public class Product : ProductInventory
    {
        public Guid Id { get; set; }
        public Guid ProductCategoryId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ProductCategory ProductCategory { get; set; }
        public List<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
        public List<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
        public List<ProductTagRelation> ProductTagRelations { get; set; } = new List<ProductTagRelation>();
    }
}
