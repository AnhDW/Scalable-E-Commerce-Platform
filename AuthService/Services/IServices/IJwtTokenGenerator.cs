using AuthService.Entities;
using System.Security.Claims;

namespace AuthService.Services.IServices
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles);
        ClaimsPrincipal ValidateToken(string token);
    }
}
