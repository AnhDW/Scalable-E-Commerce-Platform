using Common.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Contracts.DTOs.Relationship.Handle
{
    public class UpdateStoresByUserDto
    {
        public string UserId { get; set; }
        public List<(Guid StoreId, StoreRole StoreRole)> StoreIdsAndStoreRoles { get; set; } = new List<(Guid StoreId, StoreRole StoreRole)>();
    }
}
