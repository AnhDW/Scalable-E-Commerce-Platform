using AuthRepositories.Repositories.IRepositories;
using AuthService.Repositories.IRepositories;
using AutoMapper;
using Common.Extensions;
using Common.Helper.EntityParams;
using Contracts.DTOs;
using Contracts.DTOs.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly ISharedRepository _sharedRepository;
        private readonly IMapper _mapper;
        protected ResponseDto _responseDto;
        public PermissionsController(IPermissionRepository permissionRepository, IMapper mapper, ISharedRepository sharedRepository)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
            _responseDto = new ResponseDto();
            _sharedRepository = sharedRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PermissionParams permissionParams)
        {
            var result = await _permissionRepository.GetAll(permissionParams);
            Response.AddPaginationHeader(new Common.Helper.PaginationHeader(result.CurrentPage, result.PageSize,
                result.TotalCount, result.TotalPages));
            _responseDto.Result = _mapper.Map<IEnumerable<PermissionDto>>(result);
            return Ok(_responseDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var permission = await _permissionRepository.GetById(id);
            if (permission == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not Found";
                return NotFound(_responseDto);
            }
            _responseDto.Result = _mapper.Map<PermissionDto>(permission);
            return Ok(_responseDto);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] PermissionDto permissionDto)
        {
            if (await _permissionRepository.CheckExists(permissionDto.Code))
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Permission code already exists.";
                return BadRequest(_responseDto);
            }
            var permission = _mapper.Map<Entities.Permission>(permissionDto);
            if (await _sharedRepository.SaveAllChanges())
            {
                _responseDto.Message = "Permission created successfully.";
                return Ok(_responseDto);
            }

            _responseDto.IsSuccess = false;
            _responseDto.Message = "Failed to create permission.";
            return BadRequest(_responseDto);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] PermissionDto permissionDto)
        {
            var permission = await _permissionRepository.GetById(permissionDto.Id);
            if (permission == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not Found";
                return NotFound(_responseDto);
            }
            var existingPermission = (await _permissionRepository.CheckExists(permissionDto.Code)) && permission.Code != permissionDto.Code;
            if (existingPermission)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Permission code already exists.";
                return BadRequest(_responseDto);
            }
            _mapper.Map(permissionDto, permission);
            _permissionRepository.Update(permission);
            if (await _sharedRepository.SaveAllChanges())
            {
                _responseDto.Message = "Permission updated successfully.";
                return Ok(_responseDto);
            }
            _responseDto.IsSuccess = false;
            _responseDto.Message = "Failed to update permission.";
            return BadRequest(_responseDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var permission = await _permissionRepository.GetById(id);
            if (permission == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not Found";
                return NotFound(_responseDto);
            }
            _permissionRepository.Delete(permission);
            if (await _sharedRepository.SaveAllChanges())
            {
                _responseDto.Message = "Permission deleted successfully.";
                return Ok(_responseDto);
            }
            _responseDto.IsSuccess = false;
            _responseDto.Message = "Failed to delete permission.";
            return BadRequest(_responseDto);

        }
    }
}
