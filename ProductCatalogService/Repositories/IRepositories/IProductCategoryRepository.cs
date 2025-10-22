using Common.Helper;
using Common.Helper.EntityParams;
using Contracts.DTOs.ProductCatalog;
using ProductCatalogService.Entities;

namespace ProductCatalogService.Repositories.IRepositories
{
    public interface IProductCategoryRepository
    {
        Task<PagedList<ProductCategoryDto>> GetAll(ProductCategoryParams productCategoryParams);
        Task<List<ProductCategory>> GetAll();
        Task<ProductCategory> GetById(Guid id);
        void Add(ProductCategory productCategory);
        void Update(ProductCategory productCategory);
        void Delete(ProductCategory productCategory);
    }
}
