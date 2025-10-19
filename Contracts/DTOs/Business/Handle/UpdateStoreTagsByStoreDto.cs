namespace Contracts.DTOs.Business.Handle
{
    public class UpdateStoreTagsByStoreDto
    {
        public Guid StoreId { get; set; }
        public List<Guid> StoreTagIds { get; set; } = new List<Guid>();
    }
}
