using Common.Enums;

namespace PaymentService.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } 
        public string PaymentCode { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public List<PaymentTransaction> PaymentTransactions { get; set; } = new List<PaymentTransaction>();
    }
}
