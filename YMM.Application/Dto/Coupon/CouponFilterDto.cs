using YMM.Application.Dto.Common;

namespace YMM.Application.Dto.Coupon
{
    public class CouponFilterDto : PaginationParams
    {
        public bool? IsActive { get; set; }
        public string? Search { get; set; } // Code or description
        public string? DiscountType { get; set; }
    }
}
