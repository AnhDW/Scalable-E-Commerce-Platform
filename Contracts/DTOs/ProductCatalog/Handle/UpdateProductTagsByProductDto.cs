namespace Contracts.DTOs.ProductCatalog.Handle
{
    public class UpdateProductTagsByProductDto
    {
        public Guid ProductId { get; set; }
        public List<Guid> ProducTagIds { get; set; } = new List<Guid>();
    }
}
