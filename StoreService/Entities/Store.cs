namespace StoreService.Entities
{
    public class Store
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
    }
}
