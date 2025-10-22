namespace Contracts.DTOs.ProductCatalog
{
    public class ProductDto : ProductInventoryDto
    {
        public Guid Id { get; set; }
        public Guid ProductCategoryId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
