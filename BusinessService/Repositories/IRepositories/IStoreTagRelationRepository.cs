using BusinessService.Entities;
using Common.Helper;
using Common.Helper.EntityParams;
using Contracts.DTOs.Business;

namespace BusinessService.Repositories.IRepositories
{
    public interface IStoreTagRelationRepository
    {
        Task<PagedList<StoreTagRelationDto>> GetAll(StoreTagRelationParams storeTagRelationParams);
        Task<List<StoreTagRelation>> GetAll();
        Task<List<Guid>> GetStoreIdsByStoreTagId(Guid storeTagId);
        Task<List<Guid>> GetStoreTagIdsByStoreId(Guid storeId);
        Task<StoreTagRelation> GetById(Guid storeId, Guid storeTagId);
        void Add(StoreTagRelation storeTagRelation);
        void Update(StoreTagRelation storeTagRelation);
        void Delete(StoreTagRelation storeTagRelation);
    }
}
