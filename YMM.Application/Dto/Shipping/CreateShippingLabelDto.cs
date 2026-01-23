using System.ComponentModel.DataAnnotations;

namespace YMM.Application.Dto.Shipping
{
    public class CreateShippingLabelDto
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        [MaxLength(50)]
        public string ShippingMethod { get; set; } = null!;

        [MaxLength(100)]
        public string? Carrier { get; set; }
    }

    public class ShippingLabelResponseDto
    {
        public string TrackingNumber { get; set; } = null!;
        public string Carrier { get; set; } = null!;
        public string? LabelUrl { get; set; }
        public decimal ShippingCost { get; set; }
    }
}
