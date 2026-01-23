using YMM.Application.Dto.Common;

namespace YMM.Application.Dto.Review
{
    public class ReviewFilterDto : PaginationParams
    {
        public string? Status { get; set; } // Pending, Approved, Rejected
        public int? ProductId { get; set; }
        public int? Rating { get; set; }
        public string? SortBy { get; set; } = "recent"; // recent, rating, helpful
    }
}
