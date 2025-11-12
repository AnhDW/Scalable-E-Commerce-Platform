using Common.Helper;
using Common.Helper.EntityParams;
using Contracts.DTOs.Order;
using OrderService.Entities;

namespace OrderService.Repositories.IRepositories
{
    public interface IOrderItemRepository
    {
        Task<PagedList<OrderItemDto>> GetAll(OrderItemParams orderItemParams);
        Task<List<OrderItem>> GetAll();
        Task<OrderItem> GetById(Guid orderItemId);
        void Add(OrderItem orderItem);
        void Update(OrderItem orderItem);
        void Delete(OrderItem orderItem);
    }
}
