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
    public class OrderRepository : IOrderRepository
    {
        private readonly IDbContextFactory<OrderDbContext> _dbContextFactory;
        private readonly OrderDbContext _context;
        private readonly IMapper _mapper;

        public OrderRepository(IDbContextFactory<OrderDbContext> dbContextFactory, OrderDbContext context, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _context = context;
            _mapper = mapper;
        }

        public void Add(Order order)
        {
            _context.Orders.Add(order);
        }

        public void Delete(Order order)
        {
            _context.Orders.Remove(order);
        }

        public async Task<PagedList<OrderDto>> GetAll(OrderParams orderParams)
        {
            var query = _context.Orders.AsQueryable();

            return await PagedList<OrderDto>.CreateAsync(
                query.ProjectTo<OrderDto>(_mapper.ConfigurationProvider),
                orderParams.PageNumber,
                orderParams.PageSize);
        }

        public Task<List<Order>> GetAll()
        {
            return _context.Orders.ToListAsync();
        }

        public async Task<Order> GetById(Guid orderId)
        {
            return (await _context.Orders.FindAsync(orderId))!;
        }

        public void Update(Order order)
        {
            _context.Orders.Update(order);
        }
    }
}
