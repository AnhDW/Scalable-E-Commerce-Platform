namespace Contracts.DTOs.Auth
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AvatarUrl { get; set; } = "/images/default-avatar.jpg";
        public IList<string>? Roles { get; set; } = new List<string>();
    }
}
