using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Common.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            var claim = user.FindFirst(JwtRegisteredClaimNames.Name);
            return claim?.Value!;
        }

        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        }
    }
}
