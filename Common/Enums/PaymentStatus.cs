using System.ComponentModel.DataAnnotations;

namespace Common.Enums
{
    public enum PaymentStatus
    {

        [Display(Name = "Chưa thanh toán")]
        Unpaid,

        [Display(Name = "Đã thanh toán")]
        Paid,

        [Display(Name = "Thanh toán thất bại")]
        Failed,

        [Display(Name = "Đã hoàn tiền")] 
        Refunded
    }
}
