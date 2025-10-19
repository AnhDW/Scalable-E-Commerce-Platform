using Common.Enums;
using Common.Extensions;

namespace Contracts.DTOs.Relationship
{
    public class UserStoreDto
    {
        public int UserId { get; set; }
        public int StoreId { get; set; }
        public StoreRole Role { get; set; }
        public string RoleName => Role.GetDisplayName();
    }
}
