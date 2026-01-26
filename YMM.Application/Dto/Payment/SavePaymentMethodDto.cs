using System.ComponentModel.DataAnnotations;

namespace YMM.Application.Dto.Payment
{
    public class SavePaymentMethodDto
    {
        [Required]
        [MaxLength(50)]
        public string Type { get; set; } = null!; // CreditCard, DebitCard

        [Required]
        [CreditCard]
        public string CardNumber { get; set; } = null!;

        [Required]
        [Range(1, 12)]
        public int ExpiryMonth { get; set; }

        [Required]
        [Range(2024, 2100)]
        public int ExpiryYear { get; set; }

        [Required]
        [StringLength(4, MinimumLength = 3)]
        public string Cvv { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string CardHolderName { get; set; } = null!;

        public bool SetAsDefault { get; set; } = false;
    }
}
