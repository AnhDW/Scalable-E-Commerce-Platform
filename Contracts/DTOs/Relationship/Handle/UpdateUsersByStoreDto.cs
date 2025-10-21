using Common.Enums;

namespace Contracts.DTOs.Relationship.Handle
{
    public class UpdateUsersByStoreDto
    {
        public Guid StoreId { get; set; }
        public List<(string UserId, StoreRole StoreRole)> UserIdsAndStoreRoles { get; set; } = new List<(string UserId, StoreRole StoreRole)>();

    }
}
