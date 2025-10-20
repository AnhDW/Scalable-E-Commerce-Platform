using Common.Enums;
using Common.Extensions;

namespace Contracts.DTOs.Relationship
{
    public class UserStoreRelationDto
    {
        public string UserId { get; set; }
        public Guid StoreId { get; set; }
        public StoreRole Role { get; set; }
        public string RoleName => Role.GetDisplayName();
    }
}
