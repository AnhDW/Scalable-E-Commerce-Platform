using System.ComponentModel.DataAnnotations;

namespace Common.Enums
{
    public enum StoreRole
    {
        [Display(Name = "Chủ cửa hàng")]
        Owner,

        [Display(Name = "Quản lý cửa hàng")]
        Manager,

        [Display(Name = "Nhân viên bán hàng")]
        Staff
    }
}
