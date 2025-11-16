namespace Contracts.DTOs.Relationship.Handle
{
    public class UpdateOrdersByPaymentDto
    {
        public Guid PaymentId { get; set; }
        public List<Guid> OrderIds { get; set; } = new List<Guid>();
    }
}
