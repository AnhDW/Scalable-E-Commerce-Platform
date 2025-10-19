namespace Contracts.DTOs.Business.Handle
{
    public class UpdateStoreCategoriesByStoreDto
    {
        public Guid StoreId { get; set; }
        public List<Guid> StoreCategoryIds { get; set; } = new List<Guid>();
    }
}
