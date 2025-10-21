using AutoMapper;
using BusinessService.Entities;
using BusinessService.Repositories.IRepositories;
using Common.Extensions;
using Common.Helper.EntityParams;
using Contracts.DTOs;
using Contracts.DTOs.Business.Handle;
using Microsoft.AspNetCore.Mvc;

namespace BusinessService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreCategoryRelationsController : ControllerBase
    {
        private readonly IStoreCategoryRelationRepository _storeCategoryRelationRepository;
        private readonly IStoreCategoryRepository _storeCategoryRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly ISharedRepository _sharedRepository;
        private readonly IMapper _mapper;
        protected ResponseDto _responseDto;

        public StoreCategoryRelationsController(IStoreCategoryRelationRepository storeCategoryRelationRepository, IStoreCategoryRepository storeCategoryRepository, IStoreRepository storeRepository, ISharedRepository sharedRepository, IMapper mapper)
        {
            _storeCategoryRelationRepository = storeCategoryRelationRepository;
            _storeCategoryRepository = storeCategoryRepository;
            _storeRepository = storeRepository;
            _sharedRepository = sharedRepository;
            _mapper = mapper;
            _responseDto = new ResponseDto();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] StoreCategoryRelationParams storeCategoryRelationParams)
        {
            var result = await _storeCategoryRelationRepository.GetAll(storeCategoryRelationParams);
            Response.AddPaginationHeader(new Common.Helper.PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));
            _responseDto.Result = result;
            return Ok(_responseDto);
        }

        [HttpGet("{storeId}/{storeCategoryId}")]
        public async Task<IActionResult> GetByIds(Guid storeId, Guid storeCategoryId)
        {
            var result = await _storeCategoryRelationRepository.GetById(storeId, storeCategoryId);
            if (result == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return NotFound(_responseDto);
            }
            _responseDto.Result = result;
            return Ok(_responseDto);
        }

        [HttpGet("get-stores-by-store-category-id/{storeCategoryId}")]
        public async Task<IActionResult> GetStoresByStoreCategoryId(Guid storeCategoryId)
        {
            var storeIds = await _storeCategoryRelationRepository.GetStoreIdsByStoreCategoryId(storeCategoryId);
            var stores = await _storeRepository.GetByIds(storeIds);
            _responseDto.Result = stores;
            return Ok(_responseDto);
        }

        [HttpGet("get-store-categories-by-store-id/{storeId}")]
        public async Task<IActionResult> GetStoreCategoriesByStoreId(Guid storeId)
        {
            var storeCategoryIds = await _storeCategoryRelationRepository.GetStoreCategoryIdsByStoreId(storeId);
            var storeCategories = await _storeCategoryRepository.GetByIds(storeCategoryIds);
            _responseDto.Result = storeCategories;
            return Ok(_responseDto);
        }

        [HttpPut("update-stores-by-store-category")]
        public async Task<IActionResult> UpdateStoresByStoreCategory([FromBody] UpdateStoresByStoreCategoryDto updateStoresByStoreCategoryDto)
        {
            var storeIds = await _storeCategoryRelationRepository.GetStoreIdsByStoreCategoryId(updateStoresByStoreCategoryDto.StoreCategoryId);
            var addStoreIds = updateStoresByStoreCategoryDto.StoreIds.Except(storeIds);
            var delStoreIds = storeIds.Except(updateStoresByStoreCategoryDto.StoreIds);
            foreach (var storeId in addStoreIds)
            {
                _storeCategoryRelationRepository.Add(new StoreCategoryRelation { StoreCategoryId = updateStoresByStoreCategoryDto.StoreCategoryId, StoreId = storeId });
            }
            foreach (var storeId in delStoreIds)
            {
                var storeCategoryRelation = await _storeCategoryRelationRepository.GetById(storeId, updateStoresByStoreCategoryDto.StoreCategoryId);
                _storeCategoryRelationRepository.Delete(storeCategoryRelation);
            }
            if(await _sharedRepository.SaveAllChanges())
            {
                _responseDto.Message = $"Add {addStoreIds.Count()} store category relations, delete {delStoreIds.Count()} store category relations";
                return Ok(_responseDto);
            }
            _responseDto.Message = "No change";
            return Ok(_responseDto);
        }

        [HttpPut("update-store-categories-by-store")]
        public async Task<IActionResult> UpdateStoreCategoriesByStores([FromBody] UpdateStoreCategoriesByStoreDto updateStoreCategoriesByStores)
        {
            var storeCategoryIds = await _storeCategoryRelationRepository.GetStoreCategoryIdsByStoreId(updateStoreCategoriesByStores.StoreId);
            var addStoreCategoryIds = updateStoreCategoriesByStores.StoreCategoryIds.Except(storeCategoryIds);
            var delStoreCategoryIds = storeCategoryIds.Except(updateStoreCategoriesByStores.StoreCategoryIds);
            foreach(var storeCategoryId in addStoreCategoryIds)
            {
                _storeCategoryRelationRepository.Add(new StoreCategoryRelation { StoreId = updateStoreCategoriesByStores.StoreId, StoreCategoryId = storeCategoryId });
            }
            foreach(var storeCategoryId in delStoreCategoryIds)
            {
                var storeCategoryRelation = await _storeCategoryRelationRepository.GetById(updateStoreCategoriesByStores.StoreId, storeCategoryId);
                _storeCategoryRelationRepository.Delete(storeCategoryRelation);
            }
            if (await _sharedRepository.SaveAllChanges())
            {
                _responseDto.Message = $"Add {addStoreCategoryIds.Count()} store category relations, delete {delStoreCategoryIds.Count()} store category relations";
                return Ok(_responseDto);
            }
            _responseDto.Message = "No change";
            return Ok(_responseDto);
        }
    }
}
