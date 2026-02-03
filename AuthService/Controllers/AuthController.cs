using AuthService.Entities;
using AuthService.Repositories.IRepositories;
using Common.Extensions;
using Common.Services.IServices;
using Contracts.DTOs;
using Contracts.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFileService _fileService;
        private readonly UserManager<ApplicationUser> _userManager;

        protected ResponseDto _response;
        public AuthController(IAuthRepository authRepository, IUserRepository userRepository, UserManager<ApplicationUser> userManager, IFileService fileService)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
            _userManager = userManager;
            _response = new ResponseDto();
            _fileService = fileService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegistrationRequestDto model)
        {
            if (model.file != null)
            {
                model.AvatarUrl = await _fileService.AddCompressAttachment(model.file);
            }
            if (model.Email == null)
            {
                model.Email = model.Code.Replace(" ", "") + "@gmail.com";
            }
            var registerResponse = await _authRepository.Register(model);

            if (registerResponse.Error)
            {
                _response.IsSuccess = false;
                _response.Message = registerResponse.Message;
                return BadRequest(_response);
            }
            _response.Result = registerResponse.User;
            _response.Message = "Registration Success";
            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var loginResponse = await _authRepository.Login(model);
            if (loginResponse.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Username or password incorrect";
                return BadRequest(_response);
            }
            _response.Result = loginResponse;
            return Ok(_response);
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto assignRoleDto)
        {
            var result = await _authRepository.AssignRole(assignRoleDto.UserName, assignRoleDto.RoleName);
            if (result)
            {
                _response.IsSuccess = true;
                _response.Message = "Role assigned successfully";
                return Ok(_response);
            }
            _response.IsSuccess = false;
            _response.Message = "Role assign failed";
            return BadRequest(_response);
        }

        [HttpPost("new-role")]
        public async Task<IActionResult> NewRole([FromBody] RoleDto roleDto)
        {
            var isSuccess = _authRepository.NewRole(new ApplicationRole
            {
                Name = roleDto.Name,
                NormalizedName = roleDto.NormalizedName == null || roleDto.NormalizedName == "" ? roleDto.Name ?? string.Empty.ToLower() : roleDto.NormalizedName,
                DisplayName = roleDto.DisplayName ?? string.Empty
            });
            if (!isSuccess)
            {
                _response.IsSuccess = false;
                _response.Message = "Role already exists";
                return BadRequest(_response);
            }
            _response.IsSuccess = true;
            _response.Message = "New role created successfully";
            return Ok(_response);
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var currentUser = await _userManager.FindByIdAsync(User.GetUserId());
            if (currentUser == null)
            {
                _response.IsSuccess = false;
                _response.Message = "User not found";
            }
            if (!await _authRepository.CheckPassword(currentUser!, changePasswordDto.CurrentPassword))
            {
                _response.IsSuccess = false;
                _response.Message = "Password incorrect";
                return BadRequest(_response);
            }

            if (await _authRepository.ChangePassword(currentUser!, changePasswordDto))
            {
                _response.IsSuccess = true;
                _response.Message = "Change successfully";
                return Ok(_response);
            }

            _response.IsSuccess = false;
            _response.Message = "Error during change";
            return BadRequest(_response);
        }

        [HttpPost("reset-password/{userId}")]
        public async Task<IActionResult> ResetPassword(string userId)
        {
            var user = await _userRepository.GetById(userId);
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.Message = "User not exists";
                return NotFound(_response);
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetResult = await _userManager.ResetPasswordAsync(user, resetToken, "Pa$$w0rd");

            if (resetResult.Succeeded)
            {
                _response.Message = "Password reset successfully";
                return Ok(_response);
            }

            _response.IsSuccess = false;
            _response.Message = "Error when reset password";
            return BadRequest(_response);
        }
    }
}
