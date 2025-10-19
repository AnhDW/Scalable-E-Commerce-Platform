namespace Contracts.DTOs.Business
{
    public class StoreTagRelationDto
    {
        public Guid StoreId { get; set; }
        public Guid StoreTagId { get; set; }

        public StoreDto? Store { get; set; }
        public StoreTagDto? StoreTag { get; set; }
    }
}
