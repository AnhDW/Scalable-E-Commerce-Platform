using Common.Helper;
using Common.Helper.EntityParams;
using Contracts.DTOs.ProductCatalog;
using ProductCatalogService.Entities;

namespace ProductCatalogService.Repositories.IRepositories
{
    public interface IProductTagRelationRepository
    {
        Task<PagedList<ProductTagRelationDto>> GetAll(ProductTagRelationParams productTagRelationParams);
        Task<List<ProductTagRelation>> GetAll();
        Task<List<Guid>> GetProductIdsByProductTagId(Guid productTagId);
        Task<List<Guid>> GetProductTagIdsByProductId(Guid productId);
        Task<ProductTagRelation> GetById(Guid productId, Guid productTagId);
        void Add(ProductTagRelation productTagRelation);
        void Update(ProductTagRelation productTagRelation);
        void Delete(ProductTagRelation productTagRelation);
    }
}
