using System.ComponentModel.DataAnnotations;

namespace YMM.Application.Dto.Shipping
{
    public class CalculateShippingDto
    {
        [Required]
        [MaxLength(100)]
        public string City { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string PostalCode { get; set; } = null!;

        [Range(0.01, double.MaxValue)]
        public decimal? Weight { get; set; }

        [MaxLength(50)]
        public string? ShippingMethod { get; set; }
    }

    public class ShippingCostResponseDto
    {
        public decimal Cost { get; set; }
        public string ShippingMethod { get; set; } = null!;
        public string EstimatedDelivery { get; set; } = null!;
        public bool IsFreeShipping { get; set; }
        public decimal? FreeShippingThreshold { get; set; }
    }
}
