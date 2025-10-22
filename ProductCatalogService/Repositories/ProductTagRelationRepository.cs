using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Helper;
using Common.Helper.EntityParams;
using Contracts.DTOs.ProductCatalog;
using Microsoft.EntityFrameworkCore;
using ProductCatalogService.Data;
using ProductCatalogService.Entities;
using ProductCatalogService.Repositories.IRepositories;

namespace ProductCatalogService.Repositories
{
    public class ProductTagRelationRepository : IProductTagRelationRepository
    {
        private readonly IDbContextFactory<ProductCatalogDbContext> _dbContextFactory;
        private readonly ProductCatalogDbContext _context;
        private readonly IMapper _mapper;

        public ProductTagRelationRepository(IDbContextFactory<ProductCatalogDbContext> dbContextFactory, ProductCatalogDbContext context, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _context = context;
            _mapper = mapper;
        }

        public void Add(ProductTagRelation productTagRelation)
        {
            _context.ProductTagsRelations.Add(productTagRelation);
        }

        public void Delete(ProductTagRelation productTagRelation)
        {
            _context.ProductTagsRelations.Remove(productTagRelation);
        }

        public async Task<PagedList<ProductTagRelationDto>> GetAll(ProductTagRelationParams productTagRelationParams)
        {
            var query = _context.ProductTagsRelations.AsQueryable();

            return await PagedList<ProductTagRelationDto>.CreateAsync(
                query.AsNoTracking().ProjectTo<ProductTagRelationDto>(_mapper.ConfigurationProvider),
                productTagRelationParams.PageNumber,
                productTagRelationParams.PageSize
                );
        }

        public async Task<List<ProductTagRelation>> GetAll()
        {
            return await _context.ProductTagsRelations.ToListAsync();
        }

        public async Task<ProductTagRelation> GetById(Guid productId, Guid productTagId)
        {
            return (await _context.ProductTagsRelations.FindAsync(productId, productTagId))!;
        }


        public async Task<List<Guid>> GetProductIdsByProductTagId(Guid productTagId)
        {
            return await _context.ProductTagsRelations.Where(x => x.ProductTagId == productTagId).Select(x => x.ProductId).ToListAsync();
        }

        public async Task<List<Guid>> GetProductTagIdsByProductId(Guid productId)
        {
            return await _context.ProductTagsRelations.Where(x => x.ProductId == productId).Select(x => x.ProductTagId).ToListAsync();

        }

        public void Update(ProductTagRelation productTagRelation)
        {
            _context.ProductTagsRelations.Update(productTagRelation);
        }
    }
}
