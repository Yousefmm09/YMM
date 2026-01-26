using System.ComponentModel.DataAnnotations;

namespace YMM.Application.Dto.Payment
{
    public class ConfirmPaymentDto
    {
        [Required]
        public string PaymentIntentId { get; set; } = null!;

        [Required]
        public int OrderId { get; set; }
    }
}
