using System;
using System.ComponentModel.DataAnnotations;

namespace YMM.Data.Entities
{
    public class NewsletterSubscription
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        [Required]
        public string UnsubscribeToken { get; set; } = null!;

        public DateTime SubscribedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UnsubscribedAt { get; set; }
    }
}
