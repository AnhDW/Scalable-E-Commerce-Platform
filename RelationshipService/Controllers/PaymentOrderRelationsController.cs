using AutoMapper;
using Common.Extensions;
using Common.Helper.EntityParams;
using Contracts.DTOs;
using Contracts.DTOs.Relationship.Handle;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RelationshipService.Entities;
using RelationshipService.Repositories.IRepositories;

namespace RelationshipService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentOrderRelationsController : ControllerBase
    {
        private readonly IPaymentOrderRelationRepository _paymentOrderRelationRepository;
        private readonly ISharedRepository _sharedRepository;
        private readonly IMapper _mapper;
        protected ResponseDto _responseDto;

        public PaymentOrderRelationsController(IPaymentOrderRelationRepository paymentOrderRelationRepository, ISharedRepository sharedRepository, IMapper mapper)
        {
            _paymentOrderRelationRepository = paymentOrderRelationRepository;
            _sharedRepository = sharedRepository;
            _mapper = mapper;
            _responseDto = new ResponseDto();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(PaymentOrderRelationParams paymentOrderRelationParams)
        {
            var result = await _paymentOrderRelationRepository.GetAll(paymentOrderRelationParams);
            Response.AddPaginationHeader(new Common.Helper.PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));
            _responseDto.Result = result;
            return Ok(_responseDto);
        }

        [HttpGet("{paymentId}/{orderId}")]
        public async Task<IActionResult> GetById(Guid paymentId, Guid orderId)
        {
            var result = await _paymentOrderRelationRepository.GetById(paymentId, orderId);
            if (result == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not Found";
                return NotFound(_responseDto);
            }
            _responseDto.Result = result;
            return Ok(_responseDto);
        }

        [HttpGet("get-payment-ids-by-order-id/{orderId}")]
        public async Task<IActionResult> GetPaymentsByOrderId(Guid orderId)
        {
            var paymentIds = await _paymentOrderRelationRepository.GetPaymentIdsByOrderId(orderId);
            _responseDto.Result = paymentIds;
            return Ok(_responseDto);
        }

        [HttpGet("get-order-ids-by-payment-id/{paymentId}")]
        public async Task<IActionResult> GetOrdersByPaymentId(Guid paymentId)
        {
            var orderIds = await _paymentOrderRelationRepository.GetOrderIdsByPaymentId(paymentId);
            _responseDto.Result = orderIds;
            return Ok(_responseDto);
        }

        [HttpPut("update-payments-by-order")]
        public async Task<IActionResult> UpdatePaymentsByOrder([FromBody] UpdatePaymentsByOrderDto updatePaymentsByOrderDto)
        {
            var paymentIds = await _paymentOrderRelationRepository.GetPaymentIdsByOrderId(updatePaymentsByOrderDto.OrderId);
            var addPaymentIds = updatePaymentsByOrderDto.PaymentIds.Except(paymentIds).ToList();
            var delPaymentIds = paymentIds.Except(updatePaymentsByOrderDto.PaymentIds).ToList();
            foreach (var addPaymentId in addPaymentIds)
            {
                var paymentOrderRelation = new PaymentOrderRelation
                {
                    PaymentId = addPaymentId,
                    OrderId = updatePaymentsByOrderDto.OrderId
                };
                _paymentOrderRelationRepository.Add(paymentOrderRelation);
            }
            foreach (var delPaymentId in delPaymentIds)
            {
                var paymentOrderRelation = await _paymentOrderRelationRepository.GetById(delPaymentId, updatePaymentsByOrderDto.OrderId);
                _paymentOrderRelationRepository.Delete(paymentOrderRelation);
            }   
            if (await _sharedRepository.SaveAllChanges())
            {
                _responseDto.Message = $"Add {addPaymentIds.Count()} payment order relations, delete {delPaymentIds.Count()} payment order relations";
                return Ok(_responseDto);
            }
            _responseDto.Result = "No change";
            return Ok(_responseDto);
        }

        [HttpPut("update-orders-by-payment")]
        public async Task<IActionResult> UpdateOrdersByPayment([FromBody] UpdateOrdersByPaymentDto updateOrdersByPaymentDto)
        {
            var orderIds = await _paymentOrderRelationRepository.GetOrderIdsByPaymentId(updateOrdersByPaymentDto.PaymentId);
            var addOrderIds = updateOrdersByPaymentDto.OrderIds.Except(orderIds).ToList();
            var delOrderIds = orderIds.Except(updateOrdersByPaymentDto.OrderIds).ToList();
            foreach (var addOrderId in addOrderIds)
            {
                var paymentOrderRelation = new PaymentOrderRelation
                {
                    PaymentId = updateOrdersByPaymentDto.PaymentId,
                    OrderId = addOrderId
                };
                _paymentOrderRelationRepository.Add(paymentOrderRelation);
            }
            foreach (var delOrderId in delOrderIds)
            {
                var paymentOrderRelation = await _paymentOrderRelationRepository.GetById(updateOrdersByPaymentDto.PaymentId, delOrderId);
                _paymentOrderRelationRepository.Delete(paymentOrderRelation);
            }
            if (await _sharedRepository.SaveAllChanges())
            {
                _responseDto.Message = $"Add {addOrderIds.Count()} payment order relations, delete {delOrderIds.Count()} payment order relations";
                return Ok(_responseDto);
            }
            _responseDto.Result = "No change";
            return Ok(_responseDto);
        }
    }
}
