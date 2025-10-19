namespace BusinessService.Entities
{
    public class StoreCategoryRelation
    {
        public Guid StoreId { get; set; }
        public Guid StoreCategoryId { get; set; }

        public Store Store { get; set; }
        public StoreCategory StoreCategory { get; set; }
    }
}
