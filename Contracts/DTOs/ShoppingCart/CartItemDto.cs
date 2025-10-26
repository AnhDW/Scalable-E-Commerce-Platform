namespace Contracts.DTOs.ShoppingCart
{
    public class CartItemDto
    {
        public string UserId { get; set; }
        public Guid StoreId { get; set; }
        public Guid ProductVariantId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
