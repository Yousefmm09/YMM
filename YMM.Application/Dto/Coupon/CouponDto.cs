using System;

namespace YMM.Application.Dto.Coupon
{
    public class CouponDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string DiscountType { get; set; } = null!; // Percentage, Fixed
        public decimal DiscountValue { get; set; }
        public decimal? MinPurchaseAmount { get; set; }
        public decimal? MaxDiscountAmount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? UsageLimit { get; set; }
        public int UsageCount { get; set; }
        public int? UsageLimitPerUser { get; set; }
        public bool IsActive { get; set; }
        public bool IsExpired => EndDate.HasValue && EndDate.Value < DateTime.UtcNow;
        public bool IsValid => IsActive && !IsExpired && (!UsageLimit.HasValue || UsageCount < UsageLimit.Value);
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
