using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Review;

namespace YMM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewDto createReviewDto)
        {
            var result = await _reviewService.CreateReviewAsync(createReviewDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("GetReviewofProduct")]
        public async Task<IActionResult> GetReviewsByProductId([FromQuery] int productId)
        {
            var result = await _reviewService.GetReviewsByProductIdAsync(productId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete("delete/{reviewId}")]
        public async Task<IActionResult> DeleteReview([FromRoute] int reviewId)
        {
            var result = await _reviewService.DeleteReviewAsync(reviewId);
            if (!string.IsNullOrEmpty(result))
            {
                return Ok(new { Success = true, Message = result });
            }
            return BadRequest(new { Success = false, Message = "Failed to delete the review." });
        }
        [HttpPatch("update/{reviewId}")]
        public async Task<IActionResult> UpdateReview([FromBody] UpdateReviewDto updateReviewDto, [FromRoute] int reviewId)
        {
            var result = await _reviewService.UpdateReviewAsync(updateReviewDto, reviewId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("summary/{productId}")]
        public async Task<IActionResult> GetReviewSummaryByProductId([FromRoute] int productId)
        {
            var result = await _reviewService.GetReviewSummaryByProductIdAsync(productId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("mark-helpful/{reviewId}")]
        public async Task<IActionResult> MarkReviewAsHelpful([FromRoute] int reviewId)
        {
            var result = await _reviewService.MarkReviewAsHelpfulAsync(reviewId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("unmark-helpful/{reviewId}")]
        public async Task<IActionResult> UnmarkReviewAsHelpful([FromRoute] int reviewId)
        {
            var result = await _reviewService.UnmarkReviewAsHelpfulAsync(reviewId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("ApproveReview/{Id}")]
        public async Task<IActionResult> ApproveReview([FromRoute] int Id)
        {
            if (ModelState.IsValid)
            {
                var result = await _reviewService.ApproveReview(Id);
                return Ok(result);
            }
            return BadRequest(ModelState);
        }
    }
}
