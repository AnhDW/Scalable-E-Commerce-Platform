namespace ProductCatalogService.Entities
{
    public class ProductTag
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public List<ProductTagRelation> ProductTagRelations { get; set; } = new List<ProductTagRelation>();
    }
}
