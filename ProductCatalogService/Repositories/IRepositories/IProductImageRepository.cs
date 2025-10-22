using Common.Helper;
using Common.Helper.EntityParams;
using Contracts.DTOs.ProductCatalog;
using ProductCatalogService.Entities;

namespace ProductCatalogService.Repositories.IRepositories
{
    public interface IProductImageRepository
    {
        Task<PagedList<ProductImageDto>> GetAll(ProductImageParams productImageParams);
        Task<List<ProductImage>> GetAll();
        Task<ProductImage> GetById(Guid id);
        void Add(ProductImage productImage);
        void Update(ProductImage productImage);
        void Delete(ProductImage productImage);
    }
}
