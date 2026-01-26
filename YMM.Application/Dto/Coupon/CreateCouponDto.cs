using System;
using System.ComponentModel.DataAnnotations;

namespace YMM.Application.Dto.Coupon
{
    public class CreateCouponDto
    {
        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public string Description { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string DiscountType { get; set; } = null!; // Percentage, Fixed

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Discount value must be greater than 0")]
        public decimal DiscountValue { get; set; }

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

        public bool IsActive { get; set; } = true;
    }
}
