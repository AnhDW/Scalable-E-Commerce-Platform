using AuthService.Entities;
using Contracts.DTOs.Auth;

namespace AuthService.Repositories.IRepositories
{
    public interface IAuthRepository
    {
        Task<RegisterResponseDto> Register(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<bool> ChangePassword(ApplicationUser user, ChangePasswordDto changePasswordDto);
        Task<bool> CheckPassword(ApplicationUser user, string currentPassword);
        Task<bool> AssignRole(string email, string roleName);
        Task<bool> CheckUserRole(ApplicationUser user, string roleName);
        bool NewRole(ApplicationRole role);
    }
}
