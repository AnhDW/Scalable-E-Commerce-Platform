using AutoMapper;
using Common.Extensions;
using Common.Helper.EntityParams;
using Contracts.DTOs;
using Contracts.DTOs.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Repositories.IRepositories;

namespace PaymentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ISharedRepository _sharedRepository;
        private readonly IMapper _mapper;
        protected ResponseDto _responseDto;

        public PaymentsController(IPaymentRepository paymentRepository,
            ISharedRepository sharedRepository,
            IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _sharedRepository = sharedRepository;
            _mapper = mapper;
            _responseDto = new ResponseDto();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaymentParams paymentParams)
        {
            var result = await _paymentRepository.GetAll(paymentParams);

            Response.AddPaginationHeader(new Common.Helper.PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));
            _responseDto.Result = result;
            return Ok(_responseDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var payment = await _paymentRepository.GetById(id);
            if (payment == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return NotFound(_responseDto);
            }
            _responseDto.Result = payment;
            return Ok(_responseDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentDto paymentDto)
        {
            var payment = _mapper.Map<Entities.Payment>(paymentDto);
            _paymentRepository.Add(payment);
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
        public async Task<IActionResult> Update([FromBody] PaymentDto paymentDto)
        {
            var payment = await _paymentRepository.GetById(paymentDto.Id);
            if (payment == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return NotFound(_responseDto);
            }
            _mapper.Map(paymentDto, payment);
            _paymentRepository.Update(payment);
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
            var payment = await _paymentRepository.GetById(id);
            if (payment == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return NotFound(_responseDto);
            }
            _paymentRepository.Delete(payment);
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
