using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Dto.Response;
using YMM.Application.Dto.Review;

namespace YMM.Application.Abstract.Services
{
    public interface IReviewService
    {
        Task<ApiResponse<List<GetReviewDto>>> GetReviewsByProductIdAsync(int productId);
        Task<ApiResponse<ReviewSummaryDto>> GetReviewSummaryByProductIdAsync(int productId);
        Task<ApiResponse<CreateReviewDto>> CreateReviewAsync(CreateReviewDto createReviewDto);
        Task<ApiResponse<bool>> MarkReviewAsHelpfulAsync(int reviewId);
        Task<ApiResponse<bool>> UnmarkReviewAsHelpfulAsync(int reviewId);
        Task<string> DeleteReviewAsync(int reviewId);
        Task<ApiResponse<UpdateReviewDto>> UpdateReviewAsync(UpdateReviewDto updateReviewDto, int reviewId);
        Task<string> ApproveReview(int reviewId);
    }
}
