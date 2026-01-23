using System.ComponentModel.DataAnnotations;

namespace YMM.Application.Dto.Order
{
    public class RefundDto
    {
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(500)]
        public string Reason { get; set; } = null!;
    }
}
