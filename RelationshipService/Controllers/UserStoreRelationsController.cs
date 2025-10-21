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
    public class UserStoreRelationsController : ControllerBase
    {
        private readonly IUserStoreRelationRepository _userStoreRelationRepository;
        private readonly ISharedRepository _sharedRepository;
        private readonly IMapper _mapper;
        protected ResponseDto _responseDto;

        public UserStoreRelationsController(IUserStoreRelationRepository userStoreRelationRepository, ISharedRepository sharedRepository, IMapper mapper)
        {
            _userStoreRelationRepository = userStoreRelationRepository;
            _sharedRepository = sharedRepository;
            _mapper = mapper;
            _responseDto = new ResponseDto();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] UserStoreRelationParams userStoreRelationParams)
        {
            var result = await _userStoreRelationRepository.GetAll(userStoreRelationParams);
            Response.AddPaginationHeader(new Common.Helper.PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));
            _responseDto.Result = result;
            return Ok(_responseDto);
        }

        [HttpGet("{userId}/{storeId}")]
        public async Task<IActionResult> GetByIds(string userId, Guid storeId)
        {
            var result = await _userStoreRelationRepository.GetById(userId, storeId);
            if (result == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Not found";
                return NotFound(_responseDto);
            }
            _responseDto.Result = result;
            return Ok(_responseDto);
        }
        
        [HttpGet("get-users-by-store-id/{storeId}")]
        public async Task<IActionResult> GetUsersByStoreId(Guid storeId)
        {
            var userIds = await _userStoreRelationRepository.GetUserIdsByStoreId(storeId);
            _responseDto.Result = userIds;
            return Ok(_responseDto);
        }

        [HttpGet("get-stores-by-user-id/{userId}")]
        public async Task<IActionResult> GetStoresByUserId(string userId)
        {
            var storeIds = await _userStoreRelationRepository.GetStoreIdsByUserId(userId);
            _responseDto.Result = storeIds;
            return Ok(_responseDto);
        }

        [HttpPut("update-users-by-store")]
        public async Task<IActionResult> UpdateUsersByStore([FromBody] UpdateUsersByStoreDto updateUsersByStoreDto)
        {
            var userIdsAndStoreRoles = await _userStoreRelationRepository.GetUserIdsByStoreId(updateUsersByStoreDto.StoreId);
            var addUserIdsAndStoreRoles = updateUsersByStoreDto.UserIdsAndStoreRoles.Except(userIdsAndStoreRoles);
            var delUserIdsAndStoreRoles = userIdsAndStoreRoles.Except(updateUsersByStoreDto.UserIdsAndStoreRoles);
            foreach(var userIdAndStoreRole in addUserIdsAndStoreRoles)
            {
                _userStoreRelationRepository.Add(new UserStoreRelation { UserId = userIdAndStoreRole.UserId, StoreId = updateUsersByStoreDto.StoreId, Role = userIdAndStoreRole.StoreRole });
            }
            foreach(var userIdAndStoreRole in delUserIdsAndStoreRoles)
            {
                var userStoreRelation = await _userStoreRelationRepository.GetById(userIdAndStoreRole.UserId, updateUsersByStoreDto.StoreId);
                _userStoreRelationRepository.Delete(userStoreRelation);
            }
            if(await _sharedRepository.SaveAllChanges())
            {
                _responseDto.Message = $"Add {addUserIdsAndStoreRoles.Count()} user store relations, delete {delUserIdsAndStoreRoles.Count()} user store relations";
                return Ok(_responseDto);
            }
            _responseDto.Message = "No change";
            return Ok(_responseDto);
        }

        [HttpPut("update-stores-by-user")]
        public async Task<IActionResult> UpdateStoresByUser([FromBody] UpdateStoresByUserDto updateStoresByUserDto)
        {
            var storeIdsAndStoreRoles = await _userStoreRelationRepository.GetStoreIdsByUserId(updateStoresByUserDto.UserId);
            var addStoreIdsAndStoreRoles = updateStoresByUserDto.StoreIdsAndStoreRoles.Except(storeIdsAndStoreRoles);
            var delStoreIdsAndStoreRoles = storeIdsAndStoreRoles.Except(updateStoresByUserDto.StoreIdsAndStoreRoles);
            foreach (var storeIdAndStoreRole in addStoreIdsAndStoreRoles)
            {
                _userStoreRelationRepository.Add(new UserStoreRelation { UserId = updateStoresByUserDto.UserId, StoreId = storeIdAndStoreRole.StoreId, Role = storeIdAndStoreRole.StoreRole });
            }
            foreach (var storeIdAndStoreRole in delStoreIdsAndStoreRoles)
            {
                var userStoreRelation = await _userStoreRelationRepository.GetById(updateStoresByUserDto.UserId, storeIdAndStoreRole.StoreId);
                _userStoreRelationRepository.Delete(userStoreRelation);
            }
            if (await _sharedRepository.SaveAllChanges())
            {
                _responseDto.Message = $"Add {addStoreIdsAndStoreRoles.Count()} user store relations, delete {delStoreIdsAndStoreRoles.Count()} user store relations";
                return Ok(_responseDto);
            }
            _responseDto.Message = "No change";
            return Ok(_responseDto);
        }
    }
}
