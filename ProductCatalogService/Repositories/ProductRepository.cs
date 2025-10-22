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
    public class ProductRepository : IProductRepository
    {
        private readonly IDbContextFactory<ProductCatalogDbContext> _dbContextFactory;
        private readonly ProductCatalogDbContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(IDbContextFactory<ProductCatalogDbContext> dbContextFactory, ProductCatalogDbContext context, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _context = context;
            _mapper = mapper;
        }

        public void Add(Product product)
        {
            _context.Products.Add(product);
        }

        public void Delete(Product product)
        {
            _context.Products.Remove(product);
        }

        public async Task<PagedList<ProductDto>> GetAll(ProductParams productParams)
        {
            var query = _context.Products.AsQueryable();

            return await PagedList<ProductDto>.CreateAsync(
                query.AsNoTracking().ProjectTo<ProductDto>(_mapper.ConfigurationProvider),
                productParams.PageNumber,
                productParams.PageSize
                );
        }

        public async Task<List<Product>> GetAll()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetById(Guid id)
        {
            return (await _context.Products.FindAsync(id))!;
        }

        public async Task<List<Product>> GetByIds(List<Guid> ids)
        {
            return await _context.Products.Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
        }
    }
}
