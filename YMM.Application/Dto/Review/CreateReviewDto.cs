using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YMM.Application.Dto.Review
{
    public class CreateReviewDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(2000)]
        public string Comment { get; set; } = null!;

        public List<string>? Images { get; set; }
    }
}
