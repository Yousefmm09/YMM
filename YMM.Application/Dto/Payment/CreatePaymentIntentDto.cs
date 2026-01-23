using System.ComponentModel.DataAnnotations;

namespace YMM.Application.Dto.Payment
{
    public class CreatePaymentIntentDto
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        [MaxLength(50)]
        public string PaymentMethod { get; set; } = null!;
    }

    public class PaymentIntentResponseDto
    {
        public string PaymentIntentId { get; set; } = null!;
        public string ClientSecret { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = null!;
    }
}
