using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.Repositories.IRepositories;

namespace PaymentService.Repositories
{
    public class SharedRepository : ISharedRepository
    {
        private readonly IDbContextFactory<PaymentDbContext> _dbContextFactory;
        private readonly PaymentDbContext _context;
        private readonly IMapper _mapper;

        public SharedRepository(IDbContextFactory<PaymentDbContext> dbContextFactory, PaymentDbContext context, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> SaveAllChange()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
