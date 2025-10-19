namespace BusinessService.Entities
{
    public class StoreTagRelation
    {
        public Guid StoreId { get; set; }
        public Guid StoreTagId { get; set; }

        public Store Store { get; set; }
        public StoreTag StoreTag { get; set; }
    }
}
