using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Dto.Response;
using YMM.Application.Dto.Review;

namespace YMM.Application.Abstract.Repositories
{
    public interface IReview
    {
        Task<ApiResponse<List<GetReviewDto>>> GetReviewsByProductIdAsync(int productId);
        Task<ApiResponse<ReviewSummaryDto>> GetReviewSummaryByProductIdAsync(int productId,CancellationToken ct);
        Task<ApiResponse<CreateReviewDto>> CreateReviewAsync(CreateReviewDto createReviewDto, string userId);
        Task<ApiResponse<bool>> MarkReviewAsHelpfulAsync(int reviewId, string userId);
        Task<ApiResponse<bool>> UnmarkReviewAsHelpfulAsync(int reviewId, string userId);
        Task<string> DeleteReviewAsync(int reviewId, string userId);
        Task<ApiResponse<UpdateReviewDto>> UpdateReviewAsync(UpdateReviewDto updateReviewDto, string userId, int reviewId);
        Task<string> ApproveReview(int reviewId);
    }
}
