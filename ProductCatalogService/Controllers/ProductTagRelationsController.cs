using AutoMapper;
using Common.Extensions;
using Common.Helper.EntityParams;
using Contracts.DTOs;
using Contracts.DTOs.ProductCatalog.Handle;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogService.Entities;
using ProductCatalogService.Repositories.IRepositories;

namespace ProductCatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTagRelationsController : ControllerBase
    {
        private readonly IProductTagRelationRepository _productTagRelationRepository;
        private readonly IProductTagRepository _productTagRepository;
        private readonly IProductRepository _productRepository;
        private readonly ISharedRepository _sharedRepository;
        private readonly IMapper _mapper;
        protected ResponseDto _responseDto;

        public ProductTagRelationsController(IProductTagRelationRepository productTagRelationRepository, IProductTagRepository productTagRepository, IProductRepository productRepository, ISharedRepository sharedRepository, IMapper mapper)
        {
            _productTagRelationRepository = productTagRelationRepository;
            _productTagRepository = productTagRepository;
            _productRepository = productRepository;
            _sharedRepository = sharedRepository;
            _mapper = mapper;
            _responseDto = new ResponseDto();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ProductTagRelationParams productTagRelationParams)
        {
            var result = await _productTagRelationRepository.GetAll(productTagRelationParams);
            Response.AddPaginationHeader(new Common.Helper.PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));
            _responseDto.Result = result;
            return Ok(_responseDto);
        }

        [HttpGet("{productId}/{productTagId}")]
        public async Task<IActionResult> GetById(Guid productId, Guid productTagId)
        {
            var result = await _productTagRelationRepository.GetById(productId, productTagId);
            if (result == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return NotFound(_responseDto);
            }
            _responseDto.Result = result;
            return Ok(_responseDto);
        }

        [HttpGet("get-products-by-product-tag-id/{productTagId}")]
        public async Task<IActionResult> GetProductIdsByProductTagId(Guid productTagId)
        {
            var productIds = await _productTagRelationRepository.GetProductIdsByProductTagId(productTagId);
            var products = await _productRepository.GetByIds(productIds);
            _responseDto.Result = products;
            return Ok(_responseDto);
        }

        [HttpGet("get-product-tags-by-product-id/{productId}")]
        public async Task<IActionResult> GetProductTagIdsByProductId(Guid productId)
        {
            var productTagIds = await _productTagRelationRepository.GetProductTagIdsByProductId(productId);
            var productTags = await _productTagRepository.GetByIds(productTagIds);
            _responseDto.Result = productTags;
            return Ok(_responseDto);
        }

        [HttpPut("update-products-by-product-tag")]
        public async Task<IActionResult> UpdateProductsByProductTag([FromBody] UpdateProductsByProductTagDto updateProductsByProductTagDto)
        {
            var productIds = await _productTagRelationRepository.GetProductIdsByProductTagId(updateProductsByProductTagDto.ProductTagId);
            var addProductIds = updateProductsByProductTagDto.ProducIds.Except(productIds);
            var delProductIds = productIds.Except(updateProductsByProductTagDto.ProducIds);
            foreach(var productId in addProductIds)
            {
                _productTagRelationRepository.Add(new ProductTagRelation { ProductId = productId, ProductTagId = updateProductsByProductTagDto.ProductTagId });
            }
            foreach(var productId in delProductIds)
            {
                var productTagRelation = await _productTagRelationRepository.GetById(productId, updateProductsByProductTagDto.ProductTagId);
                _productTagRelationRepository.Delete(productTagRelation);
            }
            if(await _sharedRepository.SaveAllChange())
            {
                _responseDto.Message = $"Add {addProductIds.Count()} product tag relations, delete {delProductIds.Count()} product tag relations";
                return Ok(_responseDto);
            }
            _responseDto.Message = "No change";
            return Ok(_responseDto);
        }
        [HttpPut("update-product-tags-by-product")]
        public async Task<IActionResult> UpdateProductTagsByProduct([FromBody] UpdateProductTagsByProductDto updateProductTagsByProductDto)
        {
            var productTagIds = await _productTagRelationRepository.GetProductTagIdsByProductId(updateProductTagsByProductDto.ProductId);
            var addproductTagIds = updateProductTagsByProductDto.ProducTagIds.Except(productTagIds);
            var delproductTagIds = productTagIds.Except(updateProductTagsByProductDto.ProducTagIds);
            foreach (var productTagId in addproductTagIds)
            {
                _productTagRelationRepository.Add(new ProductTagRelation { ProductId = updateProductTagsByProductDto.ProductId, ProductTagId = productTagId });
            }
            foreach (var productTagId in delproductTagIds)
            {
                var productTagRelation = await _productTagRelationRepository.GetById(updateProductTagsByProductDto.ProductId, productTagId);
                _productTagRelationRepository.Delete(productTagRelation);
            }
            if (await _sharedRepository.SaveAllChange())
            {
                _responseDto.Message = $"Add {addproductTagIds.Count()} product tag relations, delete {delproductTagIds.Count()} product tag relations";
                return Ok(_responseDto);
            }
            _responseDto.Message = "No change";
            return Ok(_responseDto);
        }
    }
}
