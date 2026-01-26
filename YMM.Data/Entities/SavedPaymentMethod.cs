using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YMM.Data.Entities.Identity;

namespace YMM.Data.Entities
{
    public class SavedPaymentMethod
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Type { get; set; } = null!; // CreditCard, DebitCard

        [Required]
        [MaxLength(100)]
        public string CardHolderName { get; set; } = null!;

        [Required]
        [MaxLength(4)]
        public string Last4Digits { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Brand { get; set; } = null!; // Visa, Mastercard, etc.

        [Required]
        public int ExpiryMonth { get; set; }

        [Required]
        public int ExpiryYear { get; set; }

        public bool IsDefault { get; set; } = false;

        [Required]
        public string Token { get; set; } = null!; // Encrypted token

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
}
