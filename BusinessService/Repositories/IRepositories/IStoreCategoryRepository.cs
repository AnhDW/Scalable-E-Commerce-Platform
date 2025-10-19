using BusinessService.Entities;
using Common.Helper;
using Common.Helper.EntityParams;
using Contracts.DTOs.Business;

namespace BusinessService.Repositories.IRepositories
{
    public interface IStoreCategoryRepository
    {
        Task<PagedList<StoreCategoryDto>> GetAll(StoreCategoryParams storeCategoryParams);
        Task<List<StoreCategory>> GetAll();
        Task<List<StoreCategory>> GetByIds(List<Guid> ids);
        Task<StoreCategory> GetById(Guid id);
        void Add(StoreCategory storeCategory);
        void Update(StoreCategory storeCategory);
        void Delete(StoreCategory storeCategory);
    }
}
