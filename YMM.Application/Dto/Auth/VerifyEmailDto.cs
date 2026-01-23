using System.ComponentModel.DataAnnotations;

namespace YMM.Application.Dto.Auth
{
    public class VerifyEmailDto
    {
        [Required]
        public string Token { get; set; } = null!;
    }

    public class ResendVerificationDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}
