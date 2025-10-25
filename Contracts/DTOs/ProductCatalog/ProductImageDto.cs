namespace Contracts.DTOs.ProductCatalog
{
    public class ProductImageDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsMain { get; set; } = false;

        public IFormFile? ImageFile { get; set; }
    }
}
