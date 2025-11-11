using System.ComponentModel.DataAnnotations;

namespace Common.Enums
{
    public enum OrderStatus
    {
        [Display(Name = "Chờ xử lý")]
        Pending,

        [Display(Name = "Đã vận chuyển")]
        Shipped,
        
        [Display(Name = "Đã giao")]
        Delivered,
        
        [Display(Name = "Đã hủy")]
        Cancelled
    }
}
