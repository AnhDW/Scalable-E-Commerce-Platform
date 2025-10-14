using Microsoft.AspNetCore.Identity;

namespace AuthService.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Code { get; set; }
        public string FullName { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
