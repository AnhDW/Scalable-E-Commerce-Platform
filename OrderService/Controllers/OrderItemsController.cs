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
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly ISharedRepository _sharedRepository;
        private readonly IMapper _mapper;
        protected ResponseDto _responseDto;

        public OrderItemsController(IOrderItemRepository orderItemRepository, ISharedRepository sharedRepository, IMapper mapper)
        {
            _orderItemRepository = orderItemRepository;
            _sharedRepository = sharedRepository;
            _mapper = mapper;
            _responseDto = new ResponseDto();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] OrderItemParams orderItemParams)
        {
            var result = await _orderItemRepository.GetAll(orderItemParams);

            Response.AddPaginationHeader(new Common.Helper.PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));
            _responseDto.Result = result;
            return Ok(_responseDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var orderItem = await _orderItemRepository.GetById(id);
            if (orderItem == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return NotFound(_responseDto);
            }
            _responseDto.Result = orderItem;
            return Ok(_responseDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderItemDto orderItemDto)
        {
            var orderItem = _mapper.Map<Entities.OrderItem>(orderItemDto);
            _orderItemRepository.Add(orderItem);
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
        public async Task<IActionResult> Update([FromBody] OrderItemDto orderItemDto)
        {
            var orderItem = await _orderItemRepository.GetById(orderItemDto.Id);
            if (orderItem == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return NotFound(_responseDto);
            }
            _mapper.Map(orderItemDto, orderItem);
            _orderItemRepository.Update(orderItem);
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
            var orderItem = await _orderItemRepository.GetById(id);
            if (orderItem == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return NotFound(_responseDto);
            }
            _orderItemRepository.Delete(orderItem);
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
