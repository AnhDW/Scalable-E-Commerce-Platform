
namespace Contracts.DTOs.Auth
{
    public class RegistrationRequestDto
    {
        public string? Email { get; set; }
        public string? Code { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        //public string? RoleName { get; set; }
        public string? AvatarUrl { get; set; }
        public IFormFile? file { get; set; }
    }
}
