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
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileService _fileService;
        private readonly ISharedRepository _sharedRepository;
        private readonly IMapper _mapper;
        protected ResponseDto _responseDto;

        public ProductsController(IProductRepository productRepository, ISharedRepository sharedRepository, IMapper mapper, IFileService fileService)
        {
            _productRepository = productRepository;
            _sharedRepository = sharedRepository;
            _mapper = mapper;
            _responseDto = new ResponseDto();
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ProductParams productParams)
        {
            var result = await _productRepository.GetAll(productParams);
            Response.AddPaginationHeader(new Common.Helper.PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));
            _responseDto.Result = result;
            return Ok(_responseDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _productRepository.GetById(id);
            if (product == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return NotFound(_responseDto);
            }
            _responseDto.Result = product;
            return Ok(_responseDto);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] ProductDto productDto, [FromForm] List<IFormFile> productImageFiles)
        {
            List<ProductImage> productImages = new List<ProductImage>();
            if (productImageFiles.Count > 0)
            {
                foreach(var file in productImageFiles)
                {
                    productImages.Add(new ProductImage
                    {
                        Id = Guid.NewGuid(),
                        ImageUrl = await _fileService.AddCompressAttachment(file),
                    });
                }
            }
            var product = _mapper.Map<Product>(productDto);
            product.ProductImages = productImages;
            _productRepository.Add(product);
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
        public async Task<IActionResult> Update([FromBody] ProductDto productDto)
        {
            var product = await _productRepository.GetById(productDto.Id);
            if (product == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return NotFound(_responseDto);
            }
            _productRepository.Update(_mapper.Map(productDto, product));
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
            var product = await _productRepository.GetById(id);
            if (product == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return NotFound(_responseDto);
            }
            _productRepository.Delete(product);
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
