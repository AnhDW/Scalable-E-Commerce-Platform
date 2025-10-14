using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AuthService.Extensions
{
    public static class JwtBearerEventsFactory
    {
        public static JwtBearerEvents Create(UserManager<Entities.ApplicationUser> userManager)
        {
            return new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs/notification"))
                        context.Token = accessToken;
                    return Task.CompletedTask;
                },
                OnTokenValidated = async context =>
                {
                    var userId = context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
                    var tokenStamp = context.Principal?.FindFirstValue("security_stamp");

                    if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(tokenStamp))
                    {
                        context.Fail("Token không chứa thông tin cần thiết.");
                        return;
                    }

                    var user = await userManager.FindByIdAsync(userId);
                    if (user == null || user.SecurityStamp != tokenStamp)
                        context.Fail("Token không hợp lệ do SecurityStamp thay đổi.");
                }
            };
        }
    }
}