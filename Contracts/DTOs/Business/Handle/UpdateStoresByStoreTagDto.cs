namespace Contracts.DTOs.Business.Handle
{
    public class UpdateStoresByStoreTagDto
    {
        public Guid StoreTagId { get; set; }
        public List<Guid> StoreIds { get; set; } = new List<Guid>();
    }
}
