using Common.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace AuthService.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddAppAuthetication(this WebApplicationBuilder builder)
        {
            builder.Services.AddJwtAuthentication(builder.Configuration);

            builder.Services.PostConfigure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                var sp = builder.Services.BuildServiceProvider();
                var userManager = sp.GetRequiredService<UserManager<Entities.ApplicationUser>>();
                options.Events = JwtBearerEventsFactory.Create(userManager);
            });

            return builder;
        }
    }
}