using AuthRepositories.Repositories.IRepositories;
using AuthService.Repositories;
using AuthService.Repositories.IRepositories;
using AutoMapper;
using Contracts.DTOs;
using Contracts.DTOs.Auth.Handle;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionRolesController : ControllerBase
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRoleRepository _permissionRoleRepository;
        private readonly ISharedRepository _sharedRepository;
        private readonly IMapper _mapper;
        private ResponseDto _responseDto;

        public PermissionRolesController(IPermissionRepository permissionRepository, IRoleRepository roleRepository, IPermissionRoleRepository permissionRoleRepository, IMapper mapper, ISharedRepository sharedRepository)
        {
            _permissionRepository = permissionRepository;
            _roleRepository = roleRepository;
            _permissionRoleRepository = permissionRoleRepository;
            _mapper = mapper;
            _responseDto = new ResponseDto();
            _sharedRepository = sharedRepository;
        }

        [HttpGet("{permissionId}/{roleId}")]
        public async Task<IActionResult> GetById(Guid permissionId, string roleId)
        {
            var permissionRole = await _permissionRoleRepository.GetById(permissionId, roleId);
            if (permissionRole == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return Ok(_responseDto);
            }
            _responseDto.Result = permissionRole;
            return Ok(_responseDto);
        }

        [HttpGet("get-role-ids-by-permission-id/{permissionId}")]
        public async Task<IActionResult> GetRoleIdsByUserId(Guid permissionId)
        {
            var roleIds = await _permissionRoleRepository.GetRoleIdsByPermissionId(permissionId);
            _responseDto.Result = roleIds;
            return Ok(_responseDto);
        }

        [HttpGet("get-permission-ids-by-role-id/{roleId}")]
        public async Task<IActionResult> GetUserIdsByRoleId(string roleId)
        {
            var permissionIds = await _permissionRoleRepository.GetPermissionIdsByRoleId(roleId);
            _responseDto.Result = permissionIds;
            return Ok(_responseDto);
        }

        [HttpPut("update-roles-by-permission")]
        public async Task<IActionResult> UpdateRolesByPermission([FromBody] UpdateRolesByPermissionDto updateRolesByPermissionDto)
        {
            var roleIds = await _permissionRoleRepository.GetRoleIdsByPermissionId(updateRolesByPermissionDto.PermissionId);
            var addRoleIds = updateRolesByPermissionDto.RoleIds.Except(roleIds).ToList();
            var delRoleIds = roleIds.Except(updateRolesByPermissionDto.RoleIds).ToList();
            foreach (var roleId in addRoleIds)
            {
                _permissionRoleRepository.Add(new Entities.PermissionRole
                {
                    PermissionId = updateRolesByPermissionDto.PermissionId,
                    RoleId = roleId
                });
            }
            foreach (var roleId in delRoleIds)
            {
                var permissionRole = await _permissionRoleRepository.GetById(updateRolesByPermissionDto.PermissionId, roleId);
                _permissionRoleRepository.Delete(permissionRole);
            }
            if (await _sharedRepository.SaveAllChanges())
            {
                _responseDto.Message = $"Add {addRoleIds.Count()} permission role relations, delete {delRoleIds.Count()} permission role relations";
                return Ok(_responseDto);
            }
            _responseDto.Message = "No change";
            return Ok(_responseDto);
        }

        [HttpPut("update-permissions-by-role")]
        public async Task<IActionResult> UpdatePermissionsByRole([FromBody] UpdatePermissionsByRoleDto updatePermissionsByRoleDto)
        {
            var permissionIds = await _permissionRoleRepository.GetPermissionIdsByRoleId(updatePermissionsByRoleDto.RoleId);
            var addPermissionIds = updatePermissionsByRoleDto.PermissionIds.Except(permissionIds).ToList();
            var delPermissionIds = permissionIds.Except(updatePermissionsByRoleDto.PermissionIds).ToList();
            foreach (var permissionId in addPermissionIds)
            {
                _permissionRoleRepository.Add(new Entities.PermissionRole
                {
                    PermissionId = permissionId,
                    RoleId = updatePermissionsByRoleDto.RoleId
                });
            }
            foreach (var permissionId in delPermissionIds)
            {
                var permissionRole = await _permissionRoleRepository.GetById(permissionId, updatePermissionsByRoleDto.RoleId);
                _permissionRoleRepository.Delete(permissionRole);
            }
            if (await _sharedRepository.SaveAllChanges())
            {
                _responseDto.Message = $"Add {addPermissionIds.Count()} permission role relations, delete {delPermissionIds.Count()} permission role relations";
                return Ok(_responseDto);
            }
            _responseDto.Message = "No change";
            return Ok(_responseDto);
        }
    }
}
