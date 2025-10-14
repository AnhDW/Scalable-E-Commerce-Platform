namespace Contracts.DTOs.Auth
{
    public class RoleDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? NormalizedName { get; set; }
        public string? DisplayName { get; set; }
    }
}
