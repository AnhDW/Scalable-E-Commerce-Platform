using Common.Enums;
using Common.Helper;
using Common.Helper.EntityParams;
using Contracts.DTOs.Relationship;
using RelationshipService.Entities;

namespace RelationshipService.Repositories.IRepositories
{
    public interface IUserStoreRelationRepository
    {
        Task<PagedList<UserStoreRelationDto>> GetAll(UserStoreRelationParams userStoreRelationParams);
        Task<List<UserStoreRelation>> GetAll();
        Task<List<(string UserId, StoreRole StoreRole)>> GetUserIdsByStoreId(Guid storeId);
        Task<List<(Guid StoreId, StoreRole StoreRole)>> GetStoreIdsByUserId(string userId);
        Task<UserStoreRelation> GetById(string userId, Guid storeId);
        void Add(UserStoreRelation userStoreRelation);
        void Update(UserStoreRelation userStoreRelation);
        void Delete(UserStoreRelation userStoreRelation);
    }
}
