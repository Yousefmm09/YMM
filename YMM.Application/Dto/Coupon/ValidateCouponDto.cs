using System.ComponentModel.DataAnnotations;

namespace YMM.Application.Dto.Coupon
{
    public class ValidateCouponDto
    {
        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = null!;

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal CartTotal { get; set; }
    }

    public class ValidateCouponResponseDto
    {
        public bool IsValid { get; set; }
        public string? Message { get; set; }
        public string? CouponCode { get; set; }
        public string? DiscountType { get; set; }
        public decimal? DiscountValue { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? NewTotal { get; set; }
    }
}
