namespace Contracts.DTOs.Auth.Handle
{
    public class UpdateRolesByUserDto
    {
        public string UserId { get; set; }
        public List<string> RoleIds { get; set; } = new List<string>();
    }
}
