using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Helper;
using Common.Helper.EntityParams;
using Contracts.DTOs.Relationship;
using Microsoft.EntityFrameworkCore;
using RelationshipService.Data;
using RelationshipService.Entities;
using RelationshipService.Repositories.IRepositories;

namespace RelationshipService.Repositories
{
    public class PaymentOrderRelationRepository : IPaymentOrderRelationRepository
    {
        private readonly IDbContextFactory<RelationshipDbContext> _dbContextFactory;
        private readonly RelationshipDbContext _context;
        private readonly IMapper _mapper;

        public PaymentOrderRelationRepository(IDbContextFactory<RelationshipDbContext> dbContextFactory, RelationshipDbContext context, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _context = context;
            _mapper = mapper;
        }

        public void Add(PaymentOrderRelation paymentOrderRelation)
        {
            _context.PaymentOrderRelations.Add(paymentOrderRelation);
        }

        public void Delete(PaymentOrderRelation paymentOrderRelation)
        {
            _context.PaymentOrderRelations.Remove(paymentOrderRelation);
        }

        public async Task<PagedList<PaymentOrderRelationDto>> GetAll(PaymentOrderRelationParams paymentOrderRelationParams)
        {
            var query = _context.PaymentOrderRelations.AsQueryable();

            return await PagedList<PaymentOrderRelationDto>.CreateAsync(
                query.AsNoTracking().ProjectTo<PaymentOrderRelationDto>(_mapper.ConfigurationProvider),
                paymentOrderRelationParams.PageNumber,
                paymentOrderRelationParams.PageSize);
        }

        public async Task<List<PaymentOrderRelation>> GetAll()
        {
            return await _context.PaymentOrderRelations.ToListAsync();
        }

        public async Task<PaymentOrderRelation> GetById(Guid paymentId, Guid orderId)
        {
            return (await _context.PaymentOrderRelations
                .FirstOrDefaultAsync(por => por.PaymentId == paymentId && por.OrderId == orderId))!;
        }

        public async Task<List<Guid>> GetOrderIdsByPaymentId(Guid paymentId)
        {
            return await _context.PaymentOrderRelations
                .Where(por => por.PaymentId == paymentId)
                .Select(por => por.OrderId)
                .ToListAsync();
        }

        public async Task<List<Guid>> GetPaymentIdsByOrderId(Guid orderId)
        {
            return await _context.PaymentOrderRelations
                .Where(por => por.OrderId == orderId)
                .Select(por => por.PaymentId)
                .ToListAsync();
        }

        public void Update(PaymentOrderRelation paymentOrderRelation)
        {
            _context.PaymentOrderRelations.Remove(paymentOrderRelation);
        }
    }
}
