using Common.Enums;

namespace PaymentService.Entities
{
    public class PaymentTransaction
    {
        public Guid Id { get; set; }
        public Guid PaymentId { get; set; }
        public string Gateway { get; set; }
        public string GatewayTransactionId { get; set; }
        public PaymentTransactionStatus PaymentTransactionStatus { get; set; }
        public string RawResponse { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Payment Payment { get; set; }
    }
}
