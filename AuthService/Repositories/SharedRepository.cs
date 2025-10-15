using AuthRepositories.Repositories.IRepositories;
using AuthService.Data;

namespace AuthService.Repositories
{
    public class SharedRepository : ISharedRepository
    {
        private readonly AuthDbContext _context;

        public SharedRepository(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveAllChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
