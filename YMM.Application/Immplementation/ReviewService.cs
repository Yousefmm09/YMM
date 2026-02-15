using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Abstract.Repositories;
using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Response;
using YMM.Application.Dto.Review;

namespace YMM.Application.Immplementation
{
    public class ReviewService : IReviewService
    {
        private readonly IReview _reviewRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        public ReviewService(IReview reviewRepository, IHttpContextAccessor contextAccessor)
        {
            _reviewRepository = reviewRepository;
            _contextAccessor = contextAccessor;
        }
        public Task<ApiResponse<CreateReviewDto>> CreateReviewAsync(CreateReviewDto createReviewDto)
        {
            var userId = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = _reviewRepository.CreateReviewAsync(createReviewDto, userId);
            return result;
        }

        public Task<string> DeleteReviewAsync(int reviewId)
        {
            var userId = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = _reviewRepository.DeleteReviewAsync(reviewId, userId);
            return result;
        }

        public Task<ApiResponse<List<GetReviewDto>>> GetReviewsByProductIdAsync(int productId)
        {
            var result = _reviewRepository.GetReviewsByProductIdAsync(productId);
            return result;
        }
       public async Task<string> ApproveReview(int reviewId)
        {
            var res =  await _reviewRepository.ApproveReview(reviewId);
            return res;
        }
        public Task<ApiResponse<ReviewSummaryDto>> GetReviewSummaryByProductIdAsync(int productId,CancellationToken ct)
        {
            var result = _reviewRepository.GetReviewSummaryByProductIdAsync(productId,ct);
            return result;
        }

        public Task<ApiResponse<bool>> MarkReviewAsHelpfulAsync(int reviewId)
        {
            var userId = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = _reviewRepository.MarkReviewAsHelpfulAsync(reviewId, userId);
            return result;
        }

        public Task<ApiResponse<bool>> UnmarkReviewAsHelpfulAsync(int reviewId)
        {
            var userId = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = _reviewRepository.UnmarkReviewAsHelpfulAsync(reviewId, userId);
            return result;
        }

        public Task<ApiResponse<UpdateReviewDto>> UpdateReviewAsync(UpdateReviewDto updateReviewDto, int reviewId)
        {
            var userId = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = _reviewRepository.UpdateReviewAsync(updateReviewDto, userId, reviewId);
            return result;
        }
    }
}
