using BusinessService.Entities;
using Common.Helper;
using Common.Helper.EntityParams;
using Contracts.DTOs.Business;

namespace BusinessService.Repositories.IRepositories
{
    public interface IStoreTagRepository
    {
        Task<PagedList<StoreTagDto>> GetAll(StoreTagParams storeTagParams);
        Task<List<StoreTag>> GetAll();
        Task<List<StoreTag>> GetByIds(List<Guid> ids);
        Task<StoreTag> GetById(Guid id);
        void Add(StoreTag storeTag);
        void Update(StoreTag storeTag);
        void Delete(StoreTag storeTag);
    }
}
