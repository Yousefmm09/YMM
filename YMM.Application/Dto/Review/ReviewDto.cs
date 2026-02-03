using System;
using System.Collections.Generic;

namespace YMM.Application.Dto.Review
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string? UserAvatar { get; set; }
        public int OrderId { get; set; }
        public int Rating { get; set; }
        public string Title { get; set; } = null!;
        public string Comment { get; set; } = null!;
        public List<string>? Images { get; set; }
        public string Status { get; set; } = null!;
        public int HelpfulCount { get; set; }
        public bool IsHelpfulByCurrentUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class ReviewSummaryDto 
    {
        public int TotalReviews { get; set; }
        public decimal AverageRating { get; set; }
        public int FiveStarCount { get; set; }
        public int FourStarCount { get; set; }
        public int ThreeStarCount { get; set; }
        public int TwoStarCount { get; set; }
        public int OneStarCount { get; set; }
    }
}
