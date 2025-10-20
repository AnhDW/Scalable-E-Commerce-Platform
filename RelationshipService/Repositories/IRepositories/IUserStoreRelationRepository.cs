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
        Task<UserStoreRelation> GetById(int id);
        void Add(UserStoreRelation userStoreRelation);
        void Update(UserStoreRelation userStoreRelation);
        void Delete(UserStoreRelation userStoreRelation);
    }
}
