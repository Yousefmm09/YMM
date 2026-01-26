using System.ComponentModel.DataAnnotations;

namespace YMM.Application.Dto.Newsletter
{
    public class NewsletterSubscribeDto
    {
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = null!;
    }

    public class NewsletterUnsubscribeDto
    {
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = null!;

        [Required]
        public string Token { get; set; } = null!;
    }
}
