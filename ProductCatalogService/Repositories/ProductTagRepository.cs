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
    public class ProductTagRepository : IProductTagRepository
    {
        private readonly IDbContextFactory<ProductCatalogDbContext> _dbContextFactory;
        private readonly ProductCatalogDbContext _context;
        private readonly IMapper _mapper;

        public ProductTagRepository(IDbContextFactory<ProductCatalogDbContext> dbContextFactory, ProductCatalogDbContext context, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _context = context;
            _mapper = mapper;
        }

        public void Add(ProductTag productTag)
        {
            _context.ProductTags.Add(productTag);
        }

        public void Delete(ProductTag productTag)
        {
            _context.ProductTags.Remove(productTag);
        }

        public async Task<PagedList<ProductTagDto>> GetAll(ProductTagParams productTagParams)
        {
            var query = _context.ProductTags.AsQueryable();

            return await PagedList<ProductTagDto>.CreateAsync(
                query.AsNoTracking().ProjectTo<ProductTagDto>(_mapper.ConfigurationProvider),
                productTagParams.PageNumber,
                productTagParams.PageSize
                );
        }

        public async Task<List<ProductTag>> GetAll()
        {
            return await _context.ProductTags.ToListAsync();
        }

        public async Task<ProductTag> GetById(Guid id)
        {
            return (await _context.ProductTags.FindAsync(id))!;
        }

        public async Task<List<ProductTag>> GetByIds(List<Guid> ids)
        {
            return await _context.ProductTags.Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public void Update(ProductTag productTag)
        {
            _context.ProductTags.Update(productTag);
        }
    }
}
