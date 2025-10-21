using Common.Enums;

namespace RelationshipService.Entities
{
    public class UserStoreRelation
    {
        public string UserId { get; set; }
        public Guid StoreId { get; set; }
        public StoreRole Role { get; set; } = StoreRole.Staff;
    }
}
