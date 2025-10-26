using ShoppingCartService.Data;
using ShoppingCartService.Repositories.IRepositories;

namespace ShoppingCartService.Repositories
{
    public class SharedRepository : ISharedRepository
    {
        private readonly ShoppingCartDbContext _context;

        public SharedRepository(ShoppingCartDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveAllChange()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
