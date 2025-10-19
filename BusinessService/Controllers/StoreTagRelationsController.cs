using AutoMapper;
using BusinessService.Entities;
using BusinessService.Repositories;
using BusinessService.Repositories.IRepositories;
using Common.Extensions;
using Common.Helper.EntityParams;
using Contracts.DTOs;
using Contracts.DTOs.Business.Handle;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusinessService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreTagRelationsController : ControllerBase
    {
        private readonly IStoreTagRelationRepository _storeTagRelationRepository;
        private readonly IStoreTagRepository _storeTagRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly ISharedRepository _sharedRepository;
        private readonly IMapper _mapper;
        protected ResponseDto _responseDto;

        public StoreTagRelationsController(IStoreTagRelationRepository storeTagRelationRepository, IStoreTagRepository storeTagRepository, IStoreRepository storeRepository, ISharedRepository sharedRepository, IMapper mapper)
        {
            _storeTagRelationRepository = storeTagRelationRepository;
            _storeTagRepository = storeTagRepository;
            _storeRepository = storeRepository;
            _sharedRepository = sharedRepository;
            _mapper = mapper;
            _responseDto = new ResponseDto();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] StoreTagRelationParams storeTagRelationParams)
        {
            var result = await _storeTagRelationRepository.GetAll(storeTagRelationParams);
            Response.AddPaginationHeader(new Common.Helper.PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));
            _responseDto.Result = result;
            return Ok(_responseDto);
        }

        [HttpGet("{storeId}/{storeTagId}")]
        public async Task<IActionResult> GetByIds(Guid storeId, Guid storeTagId)
        {
            var result = await _storeTagRelationRepository.GetById(storeId, storeTagId);
            if (result == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return NotFound(_responseDto);
            }
            _responseDto.Result = result;
            return Ok(_responseDto);
        }

        [HttpGet("get-stores-by-store-tag-id/{storeTagId}")]
        public async Task<IActionResult> GetStoresByStoreTagId(Guid storeTagId)
        {
            var storeIds = await _storeTagRelationRepository.GetStoreIdsByStoreTagId(storeTagId);
            var stores = await _storeRepository.GetByIds(storeIds);
            _responseDto.Result = stores;
            return Ok(_responseDto);
        }

        [HttpGet("get-store-tags-by-store-id/{storeId}")]
        public async Task<IActionResult> GetStoreTagsByStoreId(Guid storeId)
        {
            var storeTagIds = await _storeTagRelationRepository.GetStoreTagIdsByStoreId(storeId);
            var storeTags = await _storeTagRepository.GetByIds(storeTagIds);
            _responseDto.Result = storeTags;
            return Ok(_responseDto);
        }

        [HttpPut("update-stores-by-store-tag")]
        public async Task<IActionResult> UpdateStoresByStoreTag([FromBody] UpdateStoresByStoreTagDto updateStoresByStoreTagDto)
        {
            var storeIds = await _storeTagRelationRepository.GetStoreIdsByStoreTagId(updateStoresByStoreTagDto.StoreTagId);
            var addStoreIds = updateStoresByStoreTagDto.StoreIds.Except(storeIds);
            var deleteStoreIds = storeIds.Except(updateStoresByStoreTagDto.StoreIds);
            foreach(var storeId in addStoreIds)
            {
                _storeTagRelationRepository.Add(new StoreTagRelation { StoreId = storeId, StoreTagId = updateStoresByStoreTagDto.StoreTagId });
            }
            foreach (var storeId in deleteStoreIds)
            {
                var storeTagRelation = await _storeTagRelationRepository.GetById(storeId, updateStoresByStoreTagDto.StoreTagId);
                _storeTagRelationRepository.Delete(storeTagRelation);
            }
            if (await _sharedRepository.SaveAllChanges())
            {
                _responseDto.Message = $"Add {addStoreIds.Count()} store tag relations, delete {deleteStoreIds.Count()} store tag relations";
                return Ok(_responseDto);
            }
            _responseDto.Message = "No change";
            return Ok(_responseDto);
        }

        [HttpPut("update-store-tags-by-store")]
        public async Task<IActionResult> UpdateStoreTagsByStore([FromBody] UpdateStoreTagsByStoreDto updateStoreTagsByStoreDto)
        {
            var storeTagIds = await _storeTagRelationRepository.GetStoreTagIdsByStoreId(updateStoreTagsByStoreDto.StoreId);
            var addStoreTagIds = updateStoreTagsByStoreDto.StoreTagIds.Except(storeTagIds);
            var deleteStoreTagIds = storeTagIds.Except(updateStoreTagsByStoreDto.StoreTagIds);
            foreach (var storeTagId in addStoreTagIds)
            {
                _storeTagRelationRepository.Add(new StoreTagRelation { StoreId = updateStoreTagsByStoreDto.StoreId, StoreTagId = storeTagId });
            }
            foreach (var storeTagId in deleteStoreTagIds)
            {
                var storeTagRelation = await _storeTagRelationRepository.GetById(updateStoreTagsByStoreDto.StoreId, storeTagId);
                _storeTagRelationRepository.Delete(storeTagRelation);
            }
            if (await _sharedRepository.SaveAllChanges())
            {
                _responseDto.Message = $"Add {addStoreTagIds.Count()} store tag relations, delete {deleteStoreTagIds.Count()} store tag relations";
                return Ok(_responseDto);
            }
            _responseDto.Message = "No change";
            return Ok(_responseDto);
        }
    }
}
