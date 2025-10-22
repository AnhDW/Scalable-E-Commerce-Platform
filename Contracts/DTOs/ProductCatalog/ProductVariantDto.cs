namespace Contracts.DTOs.ProductCatalog
{
    public class ProductVariantDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
    }
}
