using AutoMapper;
using BusinessService.Entities;
using BusinessService.Repositories;
using BusinessService.Repositories.IRepositories;
using Common.Extensions;
using Common.Helper.EntityParams;
using Contracts.DTOs;
using Contracts.DTOs.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusinessService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly IStoreRepository _storeRepository;
        private readonly ISharedRepository _sharedRepository;
        private readonly IMapper _mapper;
        protected ResponseDto _responseDto;

        public StoresController(IStoreRepository storeRepository, ISharedRepository sharedRepository, IMapper mapper)
        {
            _storeRepository = storeRepository;
            _sharedRepository = sharedRepository;
            _mapper = mapper;
            _responseDto = new ResponseDto();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] StoreParams storeParams)
        {
            var result = await _storeRepository.GetAll(storeParams);
            Response.AddPaginationHeader(new Common.Helper.PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));

            _responseDto.Result = result;
            return Ok(_responseDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _storeRepository.GetById(id);
            if (result == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return NotFound(_responseDto);
            }
            _responseDto.Result = result;
            return Ok(_responseDto);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] StoreDto storeDto)
        {
            var store = _mapper.Map<Store>(storeDto);
            store.CreatedBy = User.GetUserId();
            _storeRepository.Add(store);
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
        public async Task<IActionResult> Update([FromBody] StoreDto storeDto)
        {
            var result = await _storeRepository.GetById(storeDto.Id);
            if (result == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return NotFound(_responseDto);
            }
            _storeRepository.Update(_mapper.Map(storeDto, result));
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
            var result = await _storeRepository.GetById(id);
            if (result == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return NotFound(_responseDto);
            }
            _storeRepository.Delete(result);
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
