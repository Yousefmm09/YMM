using System;
using System.Collections.Generic;

namespace YMM.Application.Dto.Coupon
{
    public class CouponStatsDto
    {
        public int CouponId { get; set; }
        public string CouponCode { get; set; } = null!;
        public int TotalUsage { get; set; }
        public int UniqueUsers { get; set; }
        public decimal TotalDiscountGiven { get; set; }
        public decimal TotalOrderValue { get; set; }
        public int? UsageLimit { get; set; }
        public int RemainingUsage => UsageLimit.HasValue ? UsageLimit.Value - TotalUsage : -1;
        public List<CouponUsageDetailDto> RecentUsage { get; set; } = new List<CouponUsageDetailDto>();
    }

    public class CouponUsageDetailDto
    {
        public string UserEmail { get; set; } = null!;
        public string OrderNumber { get; set; } = null!;
        public decimal DiscountAmount { get; set; }
        public decimal OrderTotal { get; set; }
        public DateTime UsedAt { get; set; }
    }
}
