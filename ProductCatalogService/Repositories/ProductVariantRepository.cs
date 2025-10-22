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
    public class ProductVariantRepository : IProductVariantRepository
    {
        private readonly IDbContextFactory<ProductCatalogDbContext> _dbContextFactory;
        private readonly ProductCatalogDbContext _context;
        private readonly IMapper _mapper;

        public ProductVariantRepository(IDbContextFactory<ProductCatalogDbContext> dbContextFactory, ProductCatalogDbContext context, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _context = context;
            _mapper = mapper;
        }

        public void Add(ProductVariant productVariant)
        {
            _context.ProductVariants.Add(productVariant);
        }

        public void Delete(ProductVariant productVariant)
        {
            _context.ProductVariants.Remove(productVariant);
        }

        public async Task<PagedList<ProductVariantDto>> GetAll(ProductVariantParams productVariantParams)
        {
            var query = _context.ProductVariants.AsQueryable();

            return await PagedList<ProductVariantDto>.CreateAsync(
                query.AsNoTracking().ProjectTo<ProductVariantDto>(_mapper.ConfigurationProvider),
                productVariantParams.PageNumber,
                productVariantParams.PageSize
                );
        }

        public async Task<List<ProductVariant>> GetAll()
        {
            return await _context.ProductVariants.ToListAsync();
        }

        public async Task<ProductVariant> GetById(Guid id)
        {
            return (await _context.ProductVariants.FindAsync(id))!;
        }

        public void Update(ProductVariant productVariant)
        {
            _context.ProductVariants.Update(productVariant);
        }
    }
}
