using Common.Helper;
using Common.Helper.EntityParams;
using Contracts.DTOs.Order;
using OrderService.Entities;

namespace OrderService.Repositories.IRepositories
{
    public interface IOrderStatusHistoryRepository
    {
        Task<PagedList<OrderStatusHistoryDto>> GetAll(OrderStatusHistoryParams orderStatusHistoryParams);
        Task<List<OrderStatusHistory>> GetAll();
        Task<OrderStatusHistory> GetById(Guid orderStatusHistoryId);
        void Add(OrderStatusHistory orderStatusHistory);
        void Update(OrderStatusHistory orderStatusHistory);
        void Delete(OrderStatusHistory orderStatusHistory);
    }
}
