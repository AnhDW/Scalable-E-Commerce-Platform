using Common.Helper;
using Common.Helper.EntityParams;
using Contracts.DTOs.ProductCatalog;
using ProductCatalogService.Entities;

namespace ProductCatalogService.Repositories.IRepositories
{
    public interface IProductVariantRepository
    {
        Task<PagedList<ProductVariantDto>> GetAll(ProductVariantParams productVariantParams);
        Task<List<ProductVariant>> GetAll();
        Task<ProductVariant> GetById(Guid id);
        void Add(ProductVariant productVariant);
        void Update(ProductVariant productVariant);
        void Delete(ProductVariant productVariant);
    }
}
