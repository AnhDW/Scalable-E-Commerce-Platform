using AuthService.Entities;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Data
{
    public class DbInitializer
    {
        public static async Task SeedAsync(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
        {
            // 1. Tạo Role trước
            var roleName = "Admin";
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new ApplicationRole
                {
                    Name = roleName,
                });
            }

            // 2. Tạo tài khoản mặc định
            var defaultUser = await userManager.FindByEmailAsync("admin@example.com");
            if (defaultUser == null)
            {
                defaultUser = new ApplicationUser()
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    NormalizedEmail = "admin@example.com".ToUpper(),
                    Code = "admin",
                    FullName = "Administrator",
                    PhoneNumber = "0123456789",
                    EmailConfirmed = true,
                    AvatarUrl = "/assets/img/avatars/7.png"
                };

                // Tạo user
                await userManager.CreateAsync(defaultUser, "Admin@123");
                await userManager.AddToRoleAsync(defaultUser, roleName);
            }
        }
    } 
}
