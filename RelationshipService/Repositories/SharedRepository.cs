using Microsoft.EntityFrameworkCore;
using RelationshipService.Data;
using RelationshipService.Repositories.IRepositories;

namespace RelationshipService.Repositories
{
    public class SharedRepository : ISharedRepository
    {
        private readonly RelationshipDbContext _context;

        public SharedRepository(RelationshipDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveAllChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
