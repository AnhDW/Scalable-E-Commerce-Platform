using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Helper;
using Common.Helper.EntityParams;
using Contracts.DTOs.Order;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Entities;
using OrderService.Repositories.IRepositories;

namespace OrderService.Repositories
{
    public class OrderStatusHistoryRepository : IOrderStatusHistoryRepository
    {
        private readonly IDbContextFactory<OrderDbContext> _dbContextFactory;
        private readonly OrderDbContext _context;
        private readonly IMapper _mapper;

        public OrderStatusHistoryRepository(IDbContextFactory<OrderDbContext> dbContextFactory, OrderDbContext context, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _context = context;
            _mapper = mapper;
        }

        public void Add(OrderStatusHistory orderStatusHistory)
        {
            _context.OrderStatusHistories.Add(orderStatusHistory);
        }

        public void Delete(OrderStatusHistory orderStatusHistory)
        {
            _context.OrderStatusHistories.Remove(orderStatusHistory);
        }

        public async Task<PagedList<OrderStatusHistoryDto>> GetAll(OrderStatusHistoryParams orderStatusHistoryParams)
        {
            var query = _context.OrderStatusHistories.AsQueryable();

            return await PagedList<OrderStatusHistoryDto>.CreateAsync(
                query.ProjectTo<OrderStatusHistoryDto>(_mapper.ConfigurationProvider),
                orderStatusHistoryParams.PageNumber,
                orderStatusHistoryParams.PageSize);
        }

        public Task<List<OrderStatusHistory>> GetAll()
        {
            return _context.OrderStatusHistories.ToListAsync();
        }

        public async Task<OrderStatusHistory> GetById(Guid orderStatusHistoryId)
        {
            return (await _context.OrderStatusHistories.FindAsync(orderStatusHistoryId))!;
        }

        public void Update(OrderStatusHistory orderStatusHistory)
        {
            _context.OrderStatusHistories.Update(orderStatusHistory);
        }
    }
}
