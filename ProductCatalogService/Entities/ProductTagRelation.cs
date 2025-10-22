namespace ProductCatalogService.Entities
{
    public class ProductTagRelation
    {
        public Guid ProductId { get; set; }
        public Guid ProductTagId {  get; set; }

        public Product Product { get; set; }
        public ProductTag ProductTag { get; set; }
    }
}
