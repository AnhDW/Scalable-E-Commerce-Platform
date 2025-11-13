namespace RelationshipService.Entities
{
    public class PaymentOrderRelation
    {
        public Guid PaymentId { get; set; }
        public Guid OrderId { get; set; }
        public decimal AmountAllocated { get; set; }
    }
}
