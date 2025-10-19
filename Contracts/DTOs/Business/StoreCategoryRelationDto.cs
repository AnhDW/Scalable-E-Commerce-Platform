namespace Contracts.DTOs.Business
{
    public class StoreCategoryRelationDto
    {
        public Guid StoreId { get; set; }
        public Guid StoreCategoryId { get; set; }

        public StoreDto? Store { get; set; }
        public StoreCategoryDto? StoreCategory { get; set; }
    }
}
