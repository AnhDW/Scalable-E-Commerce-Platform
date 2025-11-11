using Common.Enums;

namespace Contracts.DTOs.Order
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public Guid StoreId { get; set; }
        public string UserId { get; set; }
        public string OrderCode { get; set; } //ORD20251012-001
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
