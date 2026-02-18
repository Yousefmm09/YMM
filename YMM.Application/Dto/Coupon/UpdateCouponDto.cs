using System;
using System.ComponentModel.DataAnnotations;

namespace YMM.Application.Dto.Coupon
{
    public class UpdateCouponDto
    {
        
        public string? Description { get; set; }

        
        public decimal? DiscountValue { get; set; }

        
        public decimal? MinPurchaseAmount { get; set; }

        public decimal? MaxDiscountAmount { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

       
        public int? UsageLimit { get; set; }

       
        public int? UsageLimitPerUser { get; set; }

        public bool? IsActive { get; set; }
    }
}
