using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Helper;
using Common.Helper.EntityParams;
using Contracts.DTOs.Payment;
using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.Entities;
using PaymentService.Repositories.IRepositories;

namespace PaymentService.Repositories
{
    public class PaymentTransactionRepository : IPaymentTransactionRepository
    {
        private readonly IDbContextFactory<PaymentDbContext> _dbContextFactory;
        private readonly PaymentDbContext _context;
        private readonly IMapper _mapper;

        public PaymentTransactionRepository(IDbContextFactory<PaymentDbContext> dbContextFactory, PaymentDbContext context, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _context = context;
            _mapper = mapper;
        }

        public void Add(PaymentTransaction paymentTransaction)
        {
            _context.PaymentTransactions.Add(paymentTransaction);
        }

        public void Delete(PaymentTransaction paymentTransaction)
        {
            _context.PaymentTransactions.Remove(paymentTransaction);
        }

        public Task<PagedList<PaymentTransactionDto>> GetAll(PaymentTransactionParams paymentTransactionParams)
        {
            var query = _context.PaymentTransactions.AsQueryable();

            return PagedList<PaymentTransactionDto>.CreateAsync(
                query.AsNoTracking().ProjectTo<PaymentTransactionDto>(_mapper.ConfigurationProvider),
                paymentTransactionParams.PageNumber,
                paymentTransactionParams.PageSize);
        }

        public async Task<List<PaymentTransaction>> GetAll()
        {
            return await _context.PaymentTransactions.ToListAsync();
        }

        public async Task<PaymentTransaction> GetById(Guid paymentTransactionId)
        {
            return (await _context.PaymentTransactions.FindAsync(paymentTransactionId))!;
        }

        public void Update(PaymentTransaction paymentTransaction)
        {
            _context.PaymentTransactions.Update(paymentTransaction);
        }
    }
}
