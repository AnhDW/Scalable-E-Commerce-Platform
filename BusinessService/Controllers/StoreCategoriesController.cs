using AutoMapper;
using BusinessService.Entities;
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
    public class StoreCategoriesController : ControllerBase
    {
        private readonly IStoreCategoryRepository _storeCategoryRepository;
        private readonly ISharedRepository _sharedRepository;
        private readonly IMapper _mapper;
        protected ResponseDto _responseDto;

        public StoreCategoriesController(IStoreCategoryRepository storeCategoryRepository, ISharedRepository sharedRepository, IMapper mapper)
        {
            _storeCategoryRepository = storeCategoryRepository;
            _sharedRepository = sharedRepository;
            _mapper = mapper;
            _responseDto = new ResponseDto();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] StoreCategoryParams storeCategoryParams)
        {
            var result = await _storeCategoryRepository.GetAll(storeCategoryParams);
            Response.AddPaginationHeader(new Common.Helper.PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));

            _responseDto.Result = result;
            return Ok(_responseDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _storeCategoryRepository.GetById(id);
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
        public async Task<IActionResult> Add([FromBody] StoreCategoryDto storeCategoryDto)
        {
            _storeCategoryRepository.Add(_mapper.Map<StoreCategory>(storeCategoryDto));
            if(await _sharedRepository.SaveAllChanges())
            {
                _responseDto.Message = "Success";
                return Ok(_responseDto);
            }
            _responseDto.IsSuccess = false;
            _responseDto.Message = "Error";
            return BadRequest(_responseDto);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] StoreCategoryDto storeCategoryDto)
        {
            var result = await _storeCategoryRepository.GetById(storeCategoryDto.Id);
            if (result == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return NotFound(_responseDto);
            }
            _storeCategoryRepository.Update(_mapper.Map(storeCategoryDto,result));
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
            var result = await _storeCategoryRepository.GetById(id);
            if (result == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return NotFound(_responseDto);
            }
            _storeCategoryRepository.Delete(result);
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
