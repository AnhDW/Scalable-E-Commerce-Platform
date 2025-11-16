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
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IDbContextFactory<PaymentDbContext> _dbContextFactory;
        private readonly PaymentDbContext _context;
        private readonly IMapper _mapper;

        public PaymentRepository(IDbContextFactory<PaymentDbContext> dbContextFactory, PaymentDbContext context, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _context = context;
            _mapper = mapper;
        }

        public void Add(Payment payment)
        {
            _context.Payments.Add(payment);
        }

        public void Delete(Payment payment)
        {
            _context.Payments.Remove(payment);
        }

        public Task<PagedList<PaymentDto>> GetAll(PaymentParams paymentParams)
        {
            var query = _context.Payments.AsQueryable();

            return PagedList<PaymentDto>.CreateAsync(
                query.AsNoTracking().ProjectTo<PaymentDto>(_mapper.ConfigurationProvider),
                paymentParams.PageNumber,
                paymentParams.PageSize);
        }

        public async Task<List<Payment>> GetAll()
        {
            return await _context.Payments.ToListAsync();
        }

        public async Task<Payment> GetById(Guid paymentId)
        {
            return (await _context.Payments.FindAsync(paymentId))!;
        }

        public void Update(Payment payment)
        {
            _context.Payments.Update(payment);
        }
    }
}
