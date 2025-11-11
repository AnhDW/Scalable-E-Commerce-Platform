using OrderService.Data;
using OrderService.Repositories.IRepositories;

namespace OrderService.Repositories
{
    public class SharedRepository : ISharedRepository
    {
        private readonly OrderDbContext _context;

        public SharedRepository(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveAllChange()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
