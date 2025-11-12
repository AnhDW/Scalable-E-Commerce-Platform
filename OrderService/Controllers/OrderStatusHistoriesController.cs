using AutoMapper;
using Common.Extensions;
using Common.Helper.EntityParams;
using Contracts.DTOs;
using Contracts.DTOs.Order;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Repositories.IRepositories;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderStatusHistoriesController : ControllerBase
    {
        private readonly IOrderStatusHistoryRepository _orderStatusHistoryRepository;
        private readonly ISharedRepository _sharedRepository;
        private readonly IMapper _mapper;
        protected ResponseDto _responseDto;

        public OrderStatusHistoriesController(IOrderStatusHistoryRepository orderStatusHistoryRepository, ISharedRepository sharedRepository, IMapper mapper)
        {
            _orderStatusHistoryRepository = orderStatusHistoryRepository;
            _sharedRepository = sharedRepository;
            _mapper = mapper;
            _responseDto = new ResponseDto();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] OrderStatusHistoryParams orderStatusHistoryParams)
        {
            var result = await _orderStatusHistoryRepository.GetAll(orderStatusHistoryParams);

            Response.AddPaginationHeader(new Common.Helper.PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));
            _responseDto.Result = result;
            return Ok(_responseDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var orderStatusHistory = await _orderStatusHistoryRepository.GetById(id);
            if (orderStatusHistory == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return NotFound(_responseDto);
            }
            _responseDto.Result = orderStatusHistory;
            return Ok(_responseDto);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] OrderStatusHistoryDto orderStatusHistoryDto)
        {
            var orderStatusHistory = _mapper.Map<Entities.OrderStatusHistory>(orderStatusHistoryDto);
            _orderStatusHistoryRepository.Add(orderStatusHistory);
            if (await _sharedRepository.SaveAllChange())
            {
                _responseDto.Message = "Success";
                return Ok(_responseDto);
            }
            _responseDto.IsSuccess = false;
            _responseDto.Message = "Error";
            return BadRequest(_responseDto);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] OrderStatusHistoryDto orderStatusHistoryDto)
        {
            var orderStatusHistory = await _orderStatusHistoryRepository.GetById(orderStatusHistoryDto.Id);
            if (orderStatusHistory == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return NotFound(_responseDto);
            }
            _mapper.Map(orderStatusHistoryDto, orderStatusHistory);
            _orderStatusHistoryRepository.Update(orderStatusHistory);
            if (await _sharedRepository.SaveAllChange())
            {
                _responseDto.Message = "Success";
                return Ok(_responseDto);
            }
            _responseDto.IsSuccess = false;
            _responseDto.Message = "Error";
            return BadRequest(_responseDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var orderStatusHistory = await _orderStatusHistoryRepository.GetById(id);
            if (orderStatusHistory == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return NotFound(_responseDto);
            }
            _orderStatusHistoryRepository.Delete(orderStatusHistory);
            if (await _sharedRepository.SaveAllChange())
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
