using Common.Helper;
using Common.Helper.EntityParams;
using Contracts.DTOs.Order;
using OrderService.Entities;

namespace OrderService.Repositories.IRepositories
{
    public interface IOrderRepository
    {
        Task<PagedList<OrderDto>> GetAll(OrderParams orderParams);
        Task<List<Order>> GetAll();
        Task<Order> GetById(Guid orderId);
        void Add(Order order);
        void Update(Order order);
        void Delete(Order order);
    }
}
