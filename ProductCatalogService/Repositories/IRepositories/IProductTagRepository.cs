using Common.Helper;
using Common.Helper.EntityParams;
using Contracts.DTOs.ProductCatalog;
using ProductCatalogService.Entities;

namespace ProductCatalogService.Repositories.IRepositories
{
    public interface IProductTagRepository
    {
        Task<PagedList<ProductTagDto>> GetAll(ProductTagParams productTagParams);
        Task<List<ProductTag>> GetAll();
        Task<List<ProductTag>> GetByIds(List<Guid> ids);
        Task<ProductTag> GetById(Guid id);
        void Add(ProductTag productTag);
        void Update(ProductTag productTag);
        void Delete(ProductTag productTag);
    }
}
