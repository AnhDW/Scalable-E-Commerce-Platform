using Common.Enums;

namespace Contracts.DTOs.Order
{
    public class OrderStatusHistoryDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public OrderStatus PreviousStatus { get; set; }
        public OrderStatus NewStatus { get; set; }
        public DateTime ChangedAt { get; set; }
        public string ChangedBy { get; set; }
    }
}
