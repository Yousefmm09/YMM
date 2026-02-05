using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Abstract.Repositories;
using YMM.Application.Dto.Response;
using YMM.Application.Dto.Review;
using YMM.Data.Entities;
using YMM.Data.Entities.Identity;
using YMM.Infrastructure.Context;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace YMM.Infrastructure.Immplementation
{
   
    public class ReviewRepo : IReview
    {
        private readonly AppDb _appDb;
        private readonly UserManager<User> _userManager;
        public ReviewRepo(AppDb appDb,UserManager<User> userManager)
        {
            _appDb=appDb;
            _userManager=userManager;
        }
        public async Task<ApiResponse<CreateReviewDto>> CreateReviewAsync(CreateReviewDto createReviewDto, string userId)
        {
            var user= await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new ApiResponse<CreateReviewDto>
                    (
                    Success: false,
                    Message: "The user is not authorized or not login",
                    Data: null,
                    Errors: new[] { "Not found User , is not authorized or not regist here,please login or regist here" },
                    TraceId:Guid.NewGuid().ToString()
                    );
            var CreatReview = new CreateReviewDto
            {
                Title= createReviewDto.Title,
                Comment=createReviewDto.Comment,
                ProductId=createReviewDto.ProductId,
                Rating=createReviewDto.Rating,
                Images=createReviewDto.Images,
            };
            var Review = new Review
            {
                
                UserId=user.Id,
                Title=CreatReview.Title,
                Comment=createReviewDto.Comment,
                ProductId=createReviewDto.ProductId,
                Status="Pending",
                Rating=createReviewDto.Rating,
                RejectionReason=null,
                CreatedAt=DateTime.UtcNow,
                HelpfulCount=0,
                Images=createReviewDto.Images,
            };
            await _appDb.AddAsync(Review);
            await _appDb.SaveChangesAsync();
            return new ApiResponse<CreateReviewDto>
                 (
                    Success: true,
                    Message: "Your review is now reviewing by Admin  ",
                    Data: CreatReview,
                    Errors: null,
                    TraceId: Guid.NewGuid().ToString()
                  );
        }

        public async Task<string> DeleteReviewAsync(int reviewId,string userId)
        {
            var getReview =  await _appDb.Reviews.Where(x => x.Id == reviewId &&
            x.Status != "Pending" &&x.UserId==userId).FirstOrDefaultAsync();
            if (getReview != null)
            {
                _appDb.Reviews.Remove(getReview);
                await _appDb.SaveChangesAsync();
                return "Your review is deleted";
            }
            return "not found your review";
        }
        public async Task<string> ApproveReview(int reviewId)
        {
            var getReview=_appDb.Reviews.Where(x=>x.Id==reviewId && x.Status=="Pending").FirstOrDefault();
            if (getReview == null)
                return "Not found Review has status Pending";
            getReview.Status = "Approve";
            await _appDb.SaveChangesAsync();
            //update Avg in  Product Table
            var getProductId = _appDb.Reviews.Where(x => x.Id == reviewId).Select(x => x.ProductId).FirstOrDefault();
            var getAllProductR = await _appDb.Reviews
                .Where( x=>x.ProductId == getProductId && x.Status == "Approve")
                .ToListAsync();
            if (getAllProductR == null)
                return "Not Found Product Review has status Approve";
            var ClacAvg = (decimal)getAllProductR.Average(x => x.Rating);

            var UpdateAvgRateing = await _appDb.Products.Where(x => x.Id == getProductId).FirstOrDefaultAsync();
            UpdateAvgRateing.AverageRating = ClacAvg;
            await _appDb.SaveChangesAsync();
            return "Approve the review is success";

        }

        public async Task<ApiResponse<List<GetReviewDto>>> GetReviewsByProductIdAsync(int productId)
        {
            var getReview = await  _appDb.Reviews.Include(x => x.Product)
                .Where(x => x.ProductId == productId && x.Status != "Pending")
                .Select(x=>  new GetReviewDto
                {
                    Id = x.Id,
                    Comment=x.Comment,
                    CreatedAt = x.CreatedAt,
                    ProductId=productId,
                    HelpfulCount=x.HelpfulCount,
                    Title=x.Title,
                    Rating=x.Rating,
                    Images=x.Images,

                }).ToListAsync();
            return new ApiResponse<List<GetReviewDto>>
                 (
                Success:getReview!=null ? true : false,
                Message: getReview!=null ? "Reviews found" : "No reviews found",
                Data:  getReview,
                Errors: getReview==null ? new[] { "No reviews found for this product." } : null,
                TraceId: Guid.NewGuid().ToString()
                );
        }

        public async Task<ApiResponse<ReviewSummaryDto>> GetReviewSummaryByProductIdAsync(int productId)
        {
            var query = _appDb.Reviews.Include(x => x.Product).Where(x=>x.ProductId==productId).AsNoTracking().AsQueryable();
            if(query.Count() < 0)
                return new ApiResponse<ReviewSummaryDto>
                (
                    Success: false,
                    Message: "Not found reviews ",
                    Data: null,
                    Errors: new[] {"not found reviews "},
                    TraceId: Guid.NewGuid().ToString()
                );
            var countofReviews = query.Count();
            var CountRating = query.Select(x => x.Rating).Count();
            // count of rating 1 to 5
            var SummryOfReview = query.Select(x => new ReviewSummaryDto
            {
                AverageRating = (decimal)Math.Round(query.Average(x => x.Rating), 2),
                FiveStarCount = query.Count(x => x.Rating == 5),
                FourStarCount = query.Count(x => x.Rating == 4),
                ThreeStarCount = query.Count(x => x.Rating == 3),
                TwoStarCount = query.Count(x => x.Rating == 2),
                OneStarCount = query.Count(x => x.Rating == 1),
                TotalReviews = countofReviews,
            });
            var result = await SummryOfReview.FirstOrDefaultAsync();
            return new ApiResponse<ReviewSummaryDto>
                (
                    Success: true,
                    Message: "Reviews summary found",
                    Data: result,
                    Errors: null,
                    TraceId: Guid.NewGuid().ToString()
                );
        }

        public Task<ApiResponse<bool>> MarkReviewAsHelpfulAsync(int reviewId, string userId)
        {
            var getReview =  _appDb.Reviews.Where(x => x.Id == reviewId).FirstOrDefault();
            if (getReview != null)
            {
                var markHelpful = new ReviewHelpful
                {
                    ReviewId = reviewId,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };
                _appDb.ReviewHelpfuls.Add(markHelpful);
                getReview.HelpfulCount += 1;
                _appDb.Reviews.Update(getReview);
                _appDb.SaveChanges();
                return Task.FromResult(new ApiResponse<bool>
                (
                    Success: true,
                    Message: "Review marked as helpful",
                    Data: true,
                    Errors: null,
                    TraceId: Guid.NewGuid().ToString()
                ));

            }
            else
            {
                return Task.FromResult(new ApiResponse<bool>
                (
                    Success: false,
                    Message: "Review not found",
                    Data: false,
                    Errors: new[] { "The review you are trying to mark as helpful does not exist." },
                    TraceId: Guid.NewGuid().ToString()
                ));
            }
        }

        public Task<ApiResponse<bool>> UnmarkReviewAsHelpfulAsync(int reviewId, string userId)
        {
            var getReview =  _appDb.Reviews.Where(x => x.Id == reviewId).FirstOrDefault();
            if (getReview != null)
            {
                var markHelpful =  _appDb.ReviewHelpfuls
                    .Where(x => x.ReviewId == reviewId && x.UserId == userId)
                    .FirstOrDefault();
                if (markHelpful != null)
                {
                    _appDb.ReviewHelpfuls.Remove(markHelpful);
                    getReview.HelpfulCount -= 1;
                    _appDb.Reviews.Update(getReview);
                    _appDb.SaveChanges();
                    return Task.FromResult(new ApiResponse<bool>
                    (
                        Success: true,
                        Message: "Review unmarked as helpful",
                        Data: true,
                        Errors: null,
                        TraceId: Guid.NewGuid().ToString()
                    ));
                }
                else
                {
                    return Task.FromResult(new ApiResponse<bool>
                    (
                        Success: false,
                        Message: "You have not marked this review as helpful",
                        Data: false,
                        Errors: new[] { "You cannot unmark a review as helpful that you have not marked as helpful." },
                        TraceId: Guid.NewGuid().ToString()
                    ));
                }
            }
            else
            {
                return Task.FromResult(new ApiResponse<bool>
                (
                    Success: false,
                    Message: "Review not found",
                    Data: false,
                    Errors: new[] { "The review you are trying to unmark as helpful does not exist." },
                    TraceId: Guid.NewGuid().ToString()
                ));
            }
        }

        public async Task<ApiResponse<UpdateReviewDto>> UpdateReviewAsync(UpdateReviewDto updateReviewDto, string userId,int reviewId)
        {
            var getReview= await _appDb.Reviews.Where(x=>x.Id== reviewId && x.UserId==userId).FirstOrDefaultAsync();

            if (getReview == null)
                return new ApiResponse<UpdateReviewDto>
                    (
                    Success: false,
                    Message: "The review is not found",
                    Data: null,
                    Errors: null,
                    TraceId: Guid.NewGuid().ToString()
                    );
            if (getReview != null)
            {

                if (updateReviewDto.Rating>0)
                    getReview.Rating = updateReviewDto.Rating;

                if (!string.IsNullOrWhiteSpace(updateReviewDto.Title))
                    getReview.Title = updateReviewDto.Title;

                if (!string.IsNullOrWhiteSpace(updateReviewDto.Comment))
                    getReview.Comment = updateReviewDto.Comment;

                if (!string.IsNullOrWhiteSpace(updateReviewDto.Images))
                    getReview.Images = updateReviewDto.Images;
               await _appDb.SaveChangesAsync();
            }
            return new ApiResponse<UpdateReviewDto>
                 (
                    Success: true,
                    Message: "Your review has been updated",
                    Data: updateReviewDto,
                    Errors: null,
                    TraceId: Guid.NewGuid().ToString()
                  );
        }
    }
}
