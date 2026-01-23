using System;
using System.ComponentModel.DataAnnotations;

namespace YMM.Application.Dto.Coupon
{
    public class UpdateCouponDto
    {
        [MaxLength(255)]
        public string? Description { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal? DiscountValue { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? MinPurchaseAmount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? MaxDiscountAmount { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Range(1, int.MaxValue)]
        public int? UsageLimit { get; set; }

        [Range(1, int.MaxValue)]
        public int? UsageLimitPerUser { get; set; }

        public bool? IsActive { get; set; }
    }
}
