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
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly IDbContextFactory<ProductCatalogDbContext> _dbContextFactory;
        private readonly ProductCatalogDbContext _context;
        private readonly IMapper _mapper;

        public ProductImageRepository(IDbContextFactory<ProductCatalogDbContext> dbContextFactory, ProductCatalogDbContext context, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _context = context;
            _mapper = mapper;
        }

        public void Add(ProductImage productImage)
        {
            _context.ProductImages.Add(productImage);
        }

        public void Delete(ProductImage productImage)
        {
            _context.ProductImages.Remove(productImage);
        }

        public async Task<PagedList<ProductImageDto>> GetAll(ProductImageParams productImageParams)
        {
            var query = _context.ProductImages.AsQueryable();

            return await PagedList<ProductImageDto>.CreateAsync(
                query.AsNoTracking().ProjectTo<ProductImageDto>(_mapper.ConfigurationProvider),
                productImageParams.PageNumber,
                productImageParams.PageSize
                );
        }

        public async Task<List<ProductImage>> GetAll()
        {
            return await _context.ProductImages.ToListAsync();
        }

        public async Task<ProductImage> GetById(Guid id)
        {
            return (await _context.ProductImages.FindAsync(id))!;
        }

        public void Update(ProductImage productImage)
        {
            _context.ProductImages.Update(productImage);
        }
    }
}
