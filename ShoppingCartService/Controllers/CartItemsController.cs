using AutoMapper;
using Common.Extensions;
using Common.Helper.EntityParams;
using Contracts.DTOs;
using Contracts.DTOs.ShoppingCart;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartService.Entities;
using ShoppingCartService.Repositories.IRepositories;

namespace ShoppingCartService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly ISharedRepository _sharedRepository;
        private readonly IMapper _mapper;
        protected ResponseDto _responseDto;

        public CartItemsController(ICartItemRepository cartItemRepository, ISharedRepository sharedRepository, IMapper mapper)
        {
            _cartItemRepository = cartItemRepository;
            _sharedRepository = sharedRepository;
            _mapper = mapper;
            _responseDto = new ResponseDto();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] CartItemParams cartItemParams)
        {
            var result = await _cartItemRepository.GetAll(cartItemParams);
            
            Response.AddPaginationHeader(new Common.Helper.PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));
            _responseDto.Result = result;

            return Ok(_responseDto);
        }

        [HttpGet("{userId}/{storeId}/{productVariantId}")]
        public async Task<IActionResult> GetById(string userId, Guid storeId, Guid productVariantId)
        {
            var cartItem = await _cartItemRepository.GetById(userId, storeId, productVariantId);
            if(cartItem == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return NotFound(_responseDto);
            }
            _responseDto.Result = cartItem;
            return Ok(_responseDto);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CartItemDto cartItemDto)
        {
            _cartItemRepository.Add(_mapper.Map<CartItem>(cartItemDto));
            if(await _sharedRepository.SaveAllChange())
            {
                _responseDto.Message = "Success";
                return Ok(_responseDto);
            }
            _responseDto.IsSuccess = false;
            _responseDto.Message = "Error";
            return BadRequest(_responseDto);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CartItemDto cartItemDto)
        {
            var cartItem = await _cartItemRepository.GetById(cartItemDto.UserId, cartItemDto.StoreId, cartItemDto.ProductVariantId);
            if (cartItem == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return NotFound(_responseDto);
            }
            _cartItemRepository.Update(_mapper.Map(cartItemDto, cartItem));
            if (await _sharedRepository.SaveAllChange())
            {
                _responseDto.Message = "Success";
                return Ok(_responseDto);
            }
            _responseDto.IsSuccess = false;
            _responseDto.Message = "Error";
            return BadRequest(_responseDto);
        }

        [HttpDelete("{userId}/{storeId}/{productVariantId}")]
        public async Task<IActionResult> Delete(string userId, Guid storeId, Guid productVariantId)
        {
            var cartItem = await _cartItemRepository.GetById(userId, storeId, productVariantId);
            if (cartItem == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return NotFound(_responseDto);
            }
            _cartItemRepository.Delete(cartItem);
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
