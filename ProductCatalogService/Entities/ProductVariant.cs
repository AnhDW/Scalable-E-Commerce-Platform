namespace ProductCatalogService.Entities
{
    public class ProductVariant
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }

        public Product Product { get; set; }
    }
}
