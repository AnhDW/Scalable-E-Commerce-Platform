namespace Contracts.DTOs.Auth.Handle
{
    public class UpdatePermissionsByRoleDto
    {
        public string RoleId { get; set; }
        public List<Guid> PermissionIds { get; set; } = new List<Guid>();
    }
}
