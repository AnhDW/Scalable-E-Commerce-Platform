namespace ProductCatalogService.Entities
{
    public class ProductCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Guid? ParentId { get; set; } 

        public List<Product>  Products { get; set; } = new List<Product>();
    }
}
