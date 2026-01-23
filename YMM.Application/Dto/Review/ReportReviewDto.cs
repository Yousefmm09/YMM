using System.ComponentModel.DataAnnotations;

namespace YMM.Application.Dto.Review
{
    public class ReportReviewDto
    {
        [Required]
        [MaxLength(500)]
        public string Reason { get; set; } = null!;
    }
}
