using ProductCatalogService.Data;
using ProductCatalogService.Repositories.IRepositories;

namespace ProductCatalogService.Repositories
{
    public class SharedRepository : ISharedRepository
    {
        private readonly ProductCatalogDbContext _context;

        public SharedRepository(ProductCatalogDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveAllChange()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
