namespace Contracts.DTOs.Business
{
    public class StoreDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? TimeZone { get; set; } // "Asia/Ho_Chi_Minh";
        public string? Currency { get; set; } = "VND";
        public string? Language { get; set; } = "vi";
        public string? ThemeColor { get; set; } = "#007bff";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
