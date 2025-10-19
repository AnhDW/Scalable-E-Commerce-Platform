using AutoMapper;
using BusinessService.Entities;
using BusinessService.Repositories;
using BusinessService.Repositories.IRepositories;
using Common.Extensions;
using Common.Helper.EntityParams;
using Contracts.DTOs;
using Contracts.DTOs.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusinessService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreTagsController : ControllerBase
    {
        private readonly IStoreTagRepository _storeTagRepository;
        private readonly ISharedRepository _sharedRepository;
        private readonly IMapper _mapper;
        protected ResponseDto _responseDto;

        public StoreTagsController(IStoreTagRepository storeTagRepository, ISharedRepository sharedRepository, IMapper mapper)
        {
            _storeTagRepository = storeTagRepository;
            _sharedRepository = sharedRepository;
            _mapper = mapper;
            _responseDto = new ResponseDto();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] StoreTagParams storeTagParams)
        {
            var result = await _storeTagRepository.GetAll(storeTagParams);
            Response.AddPaginationHeader(new Common.Helper.PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));

            _responseDto.Result = result;
            return Ok(_responseDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _storeTagRepository.GetById(id);
            if (result == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return NotFound(_responseDto);
            }
            _responseDto.Result = result;
            return Ok(_responseDto);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] StoreTagDto storeTagDto)
        {
            _storeTagRepository.Add(_mapper.Map<StoreTag>(storeTagDto));
            if (await _sharedRepository.SaveAllChanges())
            {
                _responseDto.Message = "Success";
                return Ok(_responseDto);
            }
            _responseDto.IsSuccess = false;
            _responseDto.Message = "Error";
            return BadRequest(_responseDto);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] StoreTagDto storeTagDto)
        {
            var result = await _storeTagRepository.GetById(storeTagDto.Id);
            if (result == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return NotFound(_responseDto);
            }
            _storeTagRepository.Update(_mapper.Map(storeTagDto, result));
            if (await _sharedRepository.SaveAllChanges())
            {
                _responseDto.Message = "Success";
                return Ok(_responseDto);
            }
            _responseDto.IsSuccess = false;
            _responseDto.Message = "Error";
            return BadRequest(_responseDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deleta(Guid id)
        {
            var result = await _storeTagRepository.GetById(id);
            if (result == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return NotFound(_responseDto);
            }
            _storeTagRepository.Delete(result);
            if (await _sharedRepository.SaveAllChanges())
            {
                _responseDto.Message = "Success";
                return Ok(_responseDto);
            }
            _responseDto.IsSuccess = false;
            _responseDto.Message = "Error";
            return BadRequest(_responseDto);
        }
    }
}
