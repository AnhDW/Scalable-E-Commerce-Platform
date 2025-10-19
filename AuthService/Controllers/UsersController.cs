using AuthRepositories.Repositories.IRepositories;
using AuthService.Repositories.IRepositories;
using AutoMapper;
using Azure;
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
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ISharedRepository _sharedRepository;
        private readonly IMapper _mapper;
        protected ResponseDto _responseDto;

        public UsersController(IUserRepository userRepository, ISharedRepository sharedRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _sharedRepository = sharedRepository;
            _responseDto = new ResponseDto();
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] UserParams userParams)
        {
            var result = await _userRepository.GetAll(userParams);
            Response.AddPaginationHeader(new Common.Helper.PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));
            _responseDto.Result = result;
            return Ok(_responseDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found!";
                return NotFound(_responseDto);
            }
            _responseDto.Result = _mapper.Map<UserDto>(user);
            return Ok(_responseDto);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UserDto userDto)
        {
            var user = await _userRepository.GetById(userDto.Id);
            if (user == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found!";
                return NotFound(_responseDto);
            }
            _mapper.Map(userDto, user);
            _userRepository.Update(user);
            if (await _sharedRepository.SaveAllChanges())
            {
                _responseDto.Message = "Successful";
                return Ok(_responseDto);
            }

            _responseDto.IsSuccess = false;
            _responseDto.Message = "Error";
            return BadRequest(_responseDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "User not found!";
                return NotFound(_responseDto);
            }
            //_fileService.DeleteAttachment(user.AvatarUrl!);
            _userRepository.Delete(user);
            if (await _sharedRepository.SaveAllChanges())
            {
                _responseDto.Message = "Successful";
                return Ok(_responseDto);
            }

            _responseDto.IsSuccess = false;
            _responseDto.Message = "Error";
            return BadRequest(_responseDto);
        }


    }
}
