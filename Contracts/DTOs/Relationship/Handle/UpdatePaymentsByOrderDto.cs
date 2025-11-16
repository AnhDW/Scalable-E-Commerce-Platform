namespace Contracts.DTOs.Relationship.Handle
{
    public class UpdatePaymentsByOrderDto
    {
        public Guid OrderId { get; set; }
        public List<Guid> PaymentIds { get; set; } = new List<Guid>();
    }
}
