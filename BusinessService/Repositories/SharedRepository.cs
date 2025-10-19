using BusinessService.Data;
using BusinessService.Repositories.IRepositories;

namespace BusinessService.Repositories
{
    public class SharedRepository : ISharedRepository
    {
        private readonly BusinessDbContext _context;

        public SharedRepository(BusinessDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveAllChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
