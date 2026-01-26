using System.ComponentModel.DataAnnotations;

namespace YMM.Application.Dto.Order
{
    public class CreateOrderDto
    {
        [Required]
        public int ShippingAddressId { get; set; }

        [Required]
        public int BillingAddressId { get; set; }

        [Required]
        [MaxLength(50)]
        public string PaymentMethod { get; set; } = null!; // CreditCard, DebitCard, PayPal, CashOnDelivery

        [Required]
        [MaxLength(50)]
        public string ShippingMethod { get; set; } = null!; // Standard, Express, SameDay

        [MaxLength(500)]
        public string? Notes { get; set; }
    }
}
