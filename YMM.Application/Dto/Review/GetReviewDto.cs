using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMM.Application.Dto.Review
{
    public class GetReviewDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string UserName { get; set; } = null!;
        public string? UserAvatar { get; set; }
        public int Rating { get; set; }
        public string Title { get; set; } = null!;
        public string Comment { get; set; } = null!;
        public string? Images { get; set; }
        public string Status { get; set; } = null!;
        public int HelpfulCount { get; set; }
        public bool IsHelpfulByCurrentUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
