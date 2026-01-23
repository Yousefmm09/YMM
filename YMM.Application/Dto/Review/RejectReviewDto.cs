using System.ComponentModel.DataAnnotations;

namespace YMM.Application.Dto.Review
{
    public class RejectReviewDto
    {
        [Required]
        [MaxLength(500)]
        public string Reason { get; set; } = null!;
    }
}
