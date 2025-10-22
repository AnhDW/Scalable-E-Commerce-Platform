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
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly IDbContextFactory<ProductCatalogDbContext> _dbContextFactory;
        private readonly ProductCatalogDbContext _context;
        private readonly IMapper _mapper;

        public ProductCategoryRepository(IDbContextFactory<ProductCatalogDbContext> dbContextFactory, ProductCatalogDbContext context, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _context = context;
            _mapper = mapper;
        }

        public void Add(ProductCategory productCategory)
        {
            _context.ProductCategories.Add(productCategory);
        }

        public void Delete(ProductCategory productCategory)
        {
            _context.ProductCategories.Remove(productCategory);
        }

        public async Task<PagedList<ProductCategoryDto>> GetAll(ProductCategoryParams productCategoryParams)
        {
            var query = _context.ProductCategories.AsQueryable();

            return await PagedList<ProductCategoryDto>.CreateAsync(
                query.AsNoTracking().ProjectTo<ProductCategoryDto>(_mapper.ConfigurationProvider),
                productCategoryParams.PageNumber,
                productCategoryParams.PageSize
                );
        }

        public async Task<List<ProductCategory>> GetAll()
        {
            return await _context.ProductCategories.ToListAsync();
        }

        public async Task<ProductCategory> GetById(Guid id)
        {
            return (await _context.ProductCategories.FindAsync(id))!; 
        }

        public void Update(ProductCategory productCategory)
        {
            _context.ProductCategories.Update(productCategory);
        }
    }
}
