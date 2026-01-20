namespace Contracts.DTOs.Auth.Handle
{
    public class UpdateRolesByPermissionDto
    {
        public Guid PermissionId { get; set; }
        public List<string> RoleIds { get; set; } = new List<string>();
    }
}
