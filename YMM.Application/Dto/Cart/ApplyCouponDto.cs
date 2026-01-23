using System.ComponentModel.DataAnnotations;

namespace YMM.Application.Dto.Cart
{
    public class ApplyCouponDto
    {
        [Required]
        [MaxLength(50)]
        public string CouponCode { get; set; } = null!;
    }
}
