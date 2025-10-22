using Common.Helper;
using Common.Helper.EntityParams;
using Contracts.DTOs.ProductCatalog;
using ProductCatalogService.Entities;

namespace ProductCatalogService.Repositories.IRepositories
{
    public interface IProductRepository
    {
        Task<PagedList<ProductDto>> GetAll(ProductParams productParams);
        Task<List<Product>> GetAll();
        Task<List<Product>> GetByIds(List<Guid> ids);
        Task<Product> GetById(Guid id);
        void Add(Product product);
        void Update(Product product);
        void Delete(Product product);
    }
}
