using BusinessService.Entities;
using Common.Helper;
using Common.Helper.EntityParams;
using Contracts.DTOs.Business;

namespace BusinessService.Repositories.IRepositories
{
    public interface IStoreRepository
    {
        Task<PagedList<StoreDto>> GetAll(StoreParams storeParams);
        Task<List<Store>> GetAll();
        Task<List<Store>> GetByIds(List<Guid> ids);
        Task<Store> GetById(Guid id);
        void Add(Store store);
        void Update(Store store);
        void Delete(Store store);
    }
}
