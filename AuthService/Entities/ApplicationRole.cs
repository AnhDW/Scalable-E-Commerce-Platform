using Microsoft.AspNetCore.Identity;

namespace AuthService.Entities
{
    public class ApplicationRole : IdentityRole
    {
        public string DisplayName {  get; set; } = string.Empty;
    }
}
