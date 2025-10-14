using AuthService.Data;
using AuthService.Entities;
using AuthService.Repositories.IRepositories;
using AutoMapper;
using Contracts.DTOs.Auth;
using Microsoft.AspNetCore.Identity;
using System;

namespace AuthService.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;

        public AuthRepository(AuthDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IJwtTokenGenerator jwtTokenGenerator, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email!.ToLower() == email.ToLower() || u.Code == email);
            if (user != null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    _context.Roles.Add(new ApplicationRole
                    {
                        Name = roleName,
                        NormalizedName = roleName.ToLower(),
                        DisplayName = string.Empty
                    });
                    await _context.SaveChangesAsync();
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }

            return false;
        }

        public async Task<bool> ChangePassword(ApplicationUser user, ChangePasswordDto changePasswordDto)
        {
            var change = await _userManager.ChangePasswordAsync(
                user,
                changePasswordDto.CurrentPassword,
                changePasswordDto.NewPassword);
            return change.Succeeded;
        }

        public async Task<bool> CheckPassword(ApplicationUser user, string currentPassword)
        {
            return await _userManager.CheckPasswordAsync(user, currentPassword);
        }

        public async Task<bool> CheckUserRole(ApplicationUser user, string roleName)
        {
            var isInRole = await _userManager.IsInRoleAsync(user, roleName);

            if (isInRole)
                return true;
            else
                return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _context
                .Users
                .FirstOrDefault(u => u.UserName!.ToLower() == loginRequestDto.UserName.ToLower()
                || u.Code == loginRequestDto.UserName);

            bool isValid = await _userManager.CheckPasswordAsync(user!, loginRequestDto.Password);
            if (!isValid)
            {
                return new LoginResponseDto();
            }

            var roles = await _userManager.GetRolesAsync(user!);
            var token = _jwtTokenGenerator.GenerateToken(user!, roles);

            UserDto userDto = new()
            {
                Id = user.Id,
                Email = user.Email!,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber!,
                AvatarUrl = user.AvatarUrl == null || user.AvatarUrl == "" ? "/images/default-avatar.jpg" : user.AvatarUrl,
                Roles = roles,
            };

            LoginResponseDto loginResponseDto = new()
            {
                User = userDto,
                Token = token
            };

            return loginResponseDto;
        }

        public bool NewRole(ApplicationRole role)
        {
            if (!_roleManager.RoleExistsAsync(role.Name ?? string.Empty).GetAwaiter().GetResult())
            {
                _context.Roles.Add(role);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<RegisterResponseDto> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email!.ToUpper(),
                Code = registrationRequestDto.Code!,
                FullName = registrationRequestDto.FullName!,
                PhoneNumber = registrationRequestDto.PhoneNumber,
                EmailConfirmed = true,
                AvatarUrl = registrationRequestDto.AvatarUrl
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password!);
                if (result.Succeeded)
                {
                    var userToReturn = _context.Users.First(u => u.UserName == registrationRequestDto.Email);

                    UserDto userDto = new()
                    {
                        Id = userToReturn.Id,
                        Email = registrationRequestDto.Email,
                        Code = registrationRequestDto.Code!,
                        FullName = registrationRequestDto.FullName!,
                        PhoneNumber = registrationRequestDto.PhoneNumber!,
                        AvatarUrl = registrationRequestDto.AvatarUrl
                    };

                    return new RegisterResponseDto
                    {
                        User = userDto,
                        Error = false,
                        Message = "Registration successful",
                    };
                }
                else
                {
                    return new RegisterResponseDto
                    {
                        Error = true,
                        Message = result.Errors.FirstOrDefault()!.Description
                    };
                }
            }
            catch (Exception ex)
            {
                return new RegisterResponseDto
                {
                    Error = true,
                    Message = ex.Message
                };
            }
        }
    }
}
