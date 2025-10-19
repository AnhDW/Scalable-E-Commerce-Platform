using BusinessService.Entities;
using Common.Helper;
using Common.Helper.EntityParams;
using Contracts.DTOs.Business;

namespace BusinessService.Repositories.IRepositories
{
    public interface IStoreCategoryRelationRepository
    {
        Task<PagedList<StoreCategoryRelationDto>> GetAll(StoreCategoryRelationParams storeCategoryRelationParams);
        Task<List<Guid>> GetStoreIdsByStoreCategoryId(Guid storeCategoryId);
        Task<List<Guid>> GetStoreCategoryIdsByStoreId(Guid storeId);
        Task<List<StoreCategoryRelation>> GetAll();
        Task<StoreCategoryRelation> GetById(Guid storeId, Guid storeCategoryId);
        void Add(StoreCategoryRelation storeCategoryRelation);
        void Update(StoreCategoryRelation storeCategoryRelation);
        void Delete(StoreCategoryRelation storeCategoryRelation);
    }
}
