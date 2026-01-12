namespace Contracts.DTOs.Auth.Handle
{
    public class UpdateUsersByRoleDto
    {
        public string RoleId { get; set; }
        public List<string> UserIds { get; set; } = new List<string>();
    }
}
