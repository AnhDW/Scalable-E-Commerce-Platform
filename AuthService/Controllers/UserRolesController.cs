using AuthRepositories.Repositories.IRepositories;
using AuthService.Repositories.IRepositories;
using Contracts.DTOs;
using Contracts.DTOs.Auth.Handle;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRolesController : ControllerBase
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly ISharedRepository _sharedRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        protected ResponseDto _responseDto;

        public UserRolesController(IUserRoleRepository userRoleRepository, ISharedRepository sharedRepository, IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRoleRepository = userRoleRepository;
            _sharedRepository = sharedRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _responseDto = new ResponseDto();
        }

        [HttpGet("{userId}/{roleId}")]
        public async Task<IActionResult> GetById(string userId, string roleId)
        {
            var userRole = await _userRoleRepository.GetById(userId, roleId);
            if (userRole == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return Ok(_responseDto);
            }
            _responseDto.Result = userRole;
            return Ok(_responseDto);
        }

        [HttpGet("get-role-ids-by-user-id/{userId}")]
        public async Task<IActionResult> GetRoleIdsByUserId(string userId)
        {
            var roleIds = await _userRoleRepository.GetRoleIdsByUserId(userId);
            _responseDto.Result = roleIds;
            return Ok(_responseDto);
        }

        [HttpGet("get-user-ids-by-role-id/{roleId}")]
        public async Task<IActionResult> GetUserIdsByRoleId(string roleId)
        {
            var userIds = await _userRoleRepository.GetUserIdsByRoleId(roleId);
            _responseDto.Result = userIds;
            return Ok(_responseDto);
        }

        [HttpPut("update-roles-by-user")]
        public async Task<IActionResult> UpdateRolesByUser([FromBody] UpdateRolesByUserDto updateRolesByUserDto)
        {
            var roleIds = await _userRoleRepository.GetRoleIdsByUserId(updateRolesByUserDto.UserId);
            var addRoleIds = updateRolesByUserDto.RoleIds.Except(roleIds).ToList();
            var delRoleIds = roleIds.Except(updateRolesByUserDto.RoleIds).ToList();
            foreach (var roleId in addRoleIds)
            {
                _userRoleRepository.Add(new Entities.ApplicationUserRole { RoleId = roleId, UserId = updateRolesByUserDto.UserId });
            }

            foreach (var roleId in delRoleIds)
            {
                var userRole = await _userRoleRepository.GetById(updateRolesByUserDto.UserId, roleId);
                _userRoleRepository.Delete(userRole);
            }
            if (await _sharedRepository.SaveAllChanges())
            {
                _responseDto.Message = $"Add {addRoleIds.Count()} user roles, delete {delRoleIds.Count()} user roles";
                return Ok(_responseDto);
            }
            _responseDto.Message = "No change";
            return Ok(_responseDto);
        }

        [HttpPut("update-users-by-role")]
        public async Task<IActionResult> UpdateUsersByRole([FromBody] UpdateUsersByRoleDto updateUsersByRoleDto)
        {
            var userIds = await _userRoleRepository.GetUserIdsByRoleId(updateUsersByRoleDto.RoleId);
            var addUserIds = updateUsersByRoleDto.UserIds.Except(userIds).ToList();
            var delUserIds = userIds.Except(updateUsersByRoleDto.UserIds).ToList();
            foreach (var userId in addUserIds)
            {
                _userRoleRepository.Add(new Entities.ApplicationUserRole { RoleId = updateUsersByRoleDto.RoleId, UserId = userId });
            }
            foreach (var userId in delUserIds)
            {
                var userRole = await _userRoleRepository.GetById(userId, updateUsersByRoleDto.RoleId);
                _userRoleRepository.Delete(userRole);
            }
            if (await _sharedRepository.SaveAllChanges())
            {
                _responseDto.Message = $"Add {addUserIds.Count()} user roles, delete {delUserIds.Count()} user roles";
                return Ok(_responseDto);
            }
            _responseDto.Message = "No change";
            return Ok(_responseDto);
        }
    }
}
