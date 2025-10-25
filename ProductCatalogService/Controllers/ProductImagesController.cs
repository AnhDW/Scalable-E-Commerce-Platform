using AutoMapper;
using Common.Extensions;
using Common.Helper.EntityParams;
using Common.Services.IServices;
using Contracts.DTOs;
using Contracts.DTOs.ProductCatalog;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogService.Entities;
using ProductCatalogService.Repositories.IRepositories;

namespace ProductCatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImagesController : ControllerBase
    {
        private readonly IProductImageRepository _productImageRepository;
        private readonly ISharedRepository _sharedRepository;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        protected ResponseDto _responseDto;

        public ProductImagesController(IProductImageRepository productImageRepository, ISharedRepository sharedRepository, IMapper mapper, IFileService fileService)
        {
            _productImageRepository = productImageRepository;
            _sharedRepository = sharedRepository;
            _mapper = mapper;
            _responseDto = new ResponseDto();
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ProductImageParams productImageParams)
        {
            var result = await _productImageRepository.GetAll(productImageParams);
            Response.AddPaginationHeader(new Common.Helper.PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));
            _responseDto.Result = result;
            return Ok(_responseDto);
        }

        [HttpGet("{id}")]
        public async Task <IActionResult> GetById(Guid id)
        {
            var productImage = await _productImageRepository.GetById(id);
            if (productImage == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return NotFound(_responseDto);
            }
            _responseDto.Result = productImage;
            return Ok(_responseDto);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm]ProductImageDto productImageDto)
        {
            if (productImageDto.ImageFile != null)
            {
                productImageDto.ImageUrl = await _fileService.AddCompressAttachment(productImageDto.ImageFile);
            }
            _productImageRepository.Add(_mapper.Map<ProductImage>(productImageDto));
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
        public async Task<IActionResult> Update([FromForm] ProductImageDto productImageDto)
        {
            if (productImageDto.ImageFile != null)
            {
                _fileService.DeleteAttachment(productImageDto.ImageUrl);
                productImageDto.ImageUrl = await _fileService.AddCompressAttachment(productImageDto.ImageFile);
            }
            var productImage = await _productImageRepository.GetById(productImageDto.Id);
            if (productImage == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return NotFound(_responseDto);
            }
            _productImageRepository.Update(_mapper.Map(productImageDto, productImage));
            if (await _sharedRepository.SaveAllChange())
            {
                _responseDto.Message = "Success";
                return Ok(_responseDto);
            }
            _responseDto.IsSuccess = false;
            _responseDto.Message = "Error";
            return BadRequest(_responseDto);
        }

        [HttpDelete("id")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var productImage = await _productImageRepository.GetById(id);
            if (productImage == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return NotFound(_responseDto);
            }
            _productImageRepository.Delete(productImage);
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
