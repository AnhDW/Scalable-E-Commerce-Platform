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
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly IDbContextFactory<OrderDbContext> _dbContextFactory;
        private readonly OrderDbContext _context;
        private readonly IMapper _mapper;

        public OrderItemRepository(IDbContextFactory<OrderDbContext> dbContextFactory, OrderDbContext context, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _context = context;
            _mapper = mapper;
        }

        public void Add(OrderItem orderItem)
        {
            _context.OrderItems.Add(orderItem);
        }

        public void Delete(OrderItem orderItem)
        {
            _context.OrderItems.Remove(orderItem);
        }

        public async Task<PagedList<OrderItemDto>> GetAll(OrderItemParams orderItemParams)
        {
            var query = _context.OrderItems.AsQueryable();

            return await PagedList<OrderItemDto>.CreateAsync(
                query.ProjectTo<OrderItemDto>(_mapper.ConfigurationProvider),
                orderItemParams.PageNumber,
                orderItemParams.PageSize);
        }

        public Task<List<OrderItem>> GetAll()
        {
            return _context.OrderItems.ToListAsync();
        }

        public async Task<OrderItem> GetById(Guid orderItemId)
        {
            return (await _context.OrderItems.FindAsync(orderItemId))!;
        }

        public void Update(OrderItem orderItem)
        {
            _context.OrderItems.Update(orderItem);
        }
    }
}
