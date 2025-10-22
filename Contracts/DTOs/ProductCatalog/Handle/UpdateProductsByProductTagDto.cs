namespace Contracts.DTOs.ProductCatalog.Handle
{
    public class UpdateProductsByProductTagDto
    {
        public Guid ProductTagId {  get; set; }
        public List<Guid> ProducIds { get; set; } = new List<Guid>();
    }
}
