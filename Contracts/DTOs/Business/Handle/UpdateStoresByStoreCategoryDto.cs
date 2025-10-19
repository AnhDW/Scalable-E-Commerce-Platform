namespace Contracts.DTOs.Business.Handle
{
    public class UpdateStoresByStoreCategoryDto
    {
        public Guid StoreCategoryId { get; set; }
        public List<Guid> StoreIds { get; set; } = new List<Guid>();
    }
}
