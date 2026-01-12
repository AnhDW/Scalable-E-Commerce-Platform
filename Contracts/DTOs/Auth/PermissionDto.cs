namespace Contracts.DTOs.Auth
{
    public class PermissionDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Action { get; set; }
        public string Code { get; set; }
    }
}
