using AutoMapper;
using Common.Extensions;
using Common.Helper.EntityParams;
using Contracts.DTOs;
using Contracts.DTOs.Order;
using Contracts.DTOs.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Repositories.IRepositories;

namespace PaymentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentTransactionsController : ControllerBase
    {
        private readonly IPaymentTransactionRepository _paymentTransactionRepository;
        private readonly ISharedRepository _sharedRepository;
        private readonly IMapper _mapper;
        protected ResponseDto _responseDto;

        public PaymentTransactionsController(IPaymentTransactionRepository paymentTransactionRepository,
            ISharedRepository sharedRepository,
            IMapper mapper)
        {
            _paymentTransactionRepository = paymentTransactionRepository;
            _sharedRepository = sharedRepository;
            _mapper = mapper;
            _responseDto = new ResponseDto();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaymentTransactionParams paymentTransactionParams)
        {
            var result = await _paymentTransactionRepository.GetAll(paymentTransactionParams);

            Response.AddPaginationHeader(new Common.Helper.PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));
            _responseDto.Result = result;
            return Ok(_responseDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var paymentTransaction = await _paymentTransactionRepository.GetById(id);
            if (paymentTransaction == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return NotFound(_responseDto);
            }
            _responseDto.Result = paymentTransaction;
            return Ok(_responseDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentTransactionDto paymentTransactionDto)
        {
            var paymentTransaction = _mapper.Map<Entities.PaymentTransaction>(paymentTransactionDto);
            _paymentTransactionRepository.Add(paymentTransaction);
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
        public async Task<IActionResult> Update([FromBody] PaymentTransactionDto paymentTransactionDto)
        {
            var paymentTransaction = await _paymentTransactionRepository.GetById(paymentTransactionDto.Id);
            if (paymentTransaction == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return NotFound(_responseDto);
            }
            _mapper.Map(paymentTransactionDto, paymentTransaction);
            _paymentTransactionRepository.Update(paymentTransaction);
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
            var paymentTransaction = await _paymentTransactionRepository.GetById(id);
            if (paymentTransaction == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return NotFound(_responseDto);
            }
            _paymentTransactionRepository.Delete(paymentTransaction);
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
