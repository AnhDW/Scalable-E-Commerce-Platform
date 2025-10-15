using AuthRepositories.Repositories.IRepositories;
using AuthService.Entities;
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
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;
        private readonly ISharedRepository _sharedRepository;
        private readonly IMapper _mapper;
        protected ResponseDto _responseDto;

        public RolesController(IRoleRepository roleRepository, ISharedRepository sharedRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _sharedRepository = sharedRepository;
            _mapper = mapper;
            _responseDto = new ResponseDto();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] RoleParams roleParams)
        {
            var result = await _roleRepository.GetAll(roleParams);
            Response.AddPaginationHeader(new Common.Helper.PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));

            _responseDto.Result = result;
            return Ok(_responseDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var role = await _roleRepository.GetById(id);
            if (role == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return Ok(_responseDto);
            }

            _responseDto.Result = role;
            return Ok(_responseDto);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] RoleDto roleDto)
        {
            if (await _roleRepository.CheckExists(roleDto.Name))
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Role is exists";
                return Ok(_responseDto);
            }
            roleDto.Id = Guid.NewGuid().ToString();
            _roleRepository.Add(_mapper.Map<ApplicationRole>(roleDto));
            if (await _sharedRepository.SaveAllChanges())
            {
                _responseDto.Message = "Add successful";
                return Ok(_responseDto);
            }

            _responseDto.Message = "Error";
            return BadRequest(_responseDto);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] RoleDto roleDto)
        {
            var role = await _roleRepository.GetById(roleDto.Id);
            if (role == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return Ok(_responseDto);
            }

            _roleRepository.Update(_mapper.Map(roleDto, role));
            if (await _sharedRepository.SaveAllChanges())
            {
                _responseDto.Message = "Update successful";
                return Ok(_responseDto);
            }

            _responseDto.Message = "Error";
            return BadRequest(_responseDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleRepository.GetById(id);
            if (role == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return Ok(_responseDto);
            }

            _roleRepository.Delete(role);
            if (await _sharedRepository.SaveAllChanges())
            {
                _responseDto.Message = "Delete successful";
                return Ok(_responseDto);
            }

            _responseDto.Message = "Error";
            return BadRequest(_responseDto);
        }
    }
}
