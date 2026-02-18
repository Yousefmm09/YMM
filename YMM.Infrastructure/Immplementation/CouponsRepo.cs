using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Abstract.Repositories;
using YMM.Application.Dto.Cart;
using YMM.Application.Dto.Coupon;
using YMM.Application.Dto.Response;
using YMM.Data.Entities;
using YMM.Data.Entities.Identity;
using YMM.Infrastructure.Context;
using static Azure.Core.HttpHeader;

namespace YMM.Infrastructure.Immplementation
{
    public class CouponsRepo : ICouponsRepo
    {
        private readonly AppDb _appDb;
        public CouponsRepo(AppDb appDb)
        {
            _appDb = appDb;
        }

        //public async Task<ApiResponse<string>> ApplyCouponToOrderAsync(
        //ApplyCouponDto dto,
        //string userId,
        //int cartId)
        //{
        //    using var transaction = await _appDb.Database.BeginTransactionAsync();

        //    var coupon = await _appDb.Coupons
        //        .FirstOrDefaultAsync(x => x.Code == dto.CouponCode);

        //    if (coupon == null)
        //        return Fail("Coupon not found");

        //    if (!coupon.IsActive)
        //        return Fail("Coupon is not active");

        //    if (coupon.EndDate < DateTime.UtcNow)
        //        return Fail("Coupon expired");

        //    if (coupon.UsageCount >= coupon.UsageLimit)
        //        return Fail("Coupon usage limit reached");

        //    var cart = await _appDb.Carts
        //        .FirstOrDefaultAsync(x => x.Id == cartId && x.UserId == userId);

        //    if (cart == null)
        //        return Fail("Cart not found");

        //    if (cart.Subtotal < coupon.MinPurchaseAmount)
        //        return Fail("Minimum purchase amount not reached");

        //    var alreadyUsed = await _appDb.CouponUsages
        //        .AnyAsync(x => x.CouponId == coupon.Id && x.UserId == userId);

        //    if (alreadyUsed)
        //        return Fail("Coupon already used");

        //    decimal discount = coupon.DiscountValue == coupon.DiscountValue
        //        ? cart.Subtotal * (coupon.DiscountValue / 100m)
        //        : coupon.DiscountValue;

        //    if (coupon.MaxDiscountAmount.HasValue)
        //        discount = Math.Min(discount, coupon.MaxDiscountAmount.Value);

        //    cart.Discount = discount;
        //    cart.Total = cart.Subtotal - discount;

        //    coupon.UsageCount++;

        //    _appDb.CouponUsages.Add(new CouponUsage
        //    {
        //        CouponId = coupon.Id,
        //        UserId = userId,
        //        DiscountAmount = discount,
        //        UsedAt = DateTime.UtcNow
        //    });

        //    await _appDb.SaveChangesAsync();
        //    await transaction.CommitAsync();

        //    return new ApiResponse<string>(
        //        Success: true,
        //        Message: "Coupon applied successfully",
        //        Data: null,
        //        Errors: null,
        //        TraceId: Guid.NewGuid().ToString()
        //    );

        //    ApiResponse<string> Fail(string msg) =>
        //        new(false, msg, null, new[] { msg }, Guid.NewGuid().ToString());
        //}

        public async Task<ApiResponse<string>> ApplyCouponToOrderAsync(ApplyCouponDto dto, string userId, int cartId)
        {
            var Code = await _appDb.Coupons.Where(x => x.Code == dto.CouponCode).FirstOrDefaultAsync();
            var CartUser = await _appDb.Carts.Where(x => x.UserId == userId && x.Id == cartId).FirstOrDefaultAsync();
            if (Code == null)
                return new ApiResponse<string>
                    (
                   Success: false,
                   Message: "The code is found , change the code",
                   Data: null,
                   Errors: new[] { "Change the code to added it" },
                   TraceId: Guid.NewGuid().ToString()
                   );
            if (!Code.IsActive)
                return new ApiResponse<string>
                    (
                   Success: false,
                   Message: "The code is not active now",
                   Data: null,
                   Errors: new[] { "the code not active" },
                   TraceId: Guid.NewGuid().ToString()
                   );
            if (Code.UsageLimit < 5)
                return new ApiResponse<string>
                    (
                   Success: false,
                   Message: "The code is now arrive UsageLimit ",
                   Data: null,
                   Errors: new[] { "the code not active" },
                   TraceId: Guid.NewGuid().ToString()
                   );
            if (Code.EndDate < DateTime.Now)
                return new ApiResponse<string>
                    (
                   Success: false,
                   Message: "The code is expire ",
                   Data: null,
                   Errors: new[] { "the code not active" },
                   TraceId: Guid.NewGuid().ToString()
                   );
            decimal discount =
             CartUser.Subtotal * (Code.DiscountValue);

            //if (Code.MaxDiscountAmount.HasValue)
            //    discount = Math.Min(discount, Code.MaxDiscountAmount.Value);

            CartUser.Discount = discount;
            CartUser.Total = CartUser.Subtotal - discount;

            Code.UsageCount++;
            CartUser.Discount = discount;
            if (CartUser.Subtotal < Code.MaxDiscountAmount)
            return new ApiResponse<string>
                    (
                   Success: false,
                   Message: "Minimum purchase amount not reached ",
                   Data: null,
                   Errors: new[] { "" },
                   TraceId: Guid.NewGuid().ToString()
                   );
            var CodeUsage = new CouponUsage
            {
                CouponId = Code.Id,
                UserId = CartUser.UserId,
                DiscountAmount = discount,
                UsedAt = DateTime.Now,
            };
            await _appDb.CouponUsages.AddAsync(CodeUsage);
            await _appDb.SaveChangesAsync();
            return new ApiResponse<string>
                   (
                  Success: true,
                  Message: "The Coupon is apply ",
                  Data: null,
                  Errors: null,
                  TraceId: Guid.NewGuid().ToString()
                  );
        }

        public async Task<ApiResponse<CreateCouponDto>> CreatCouponsAsync(CreateCouponDto dto)
        {
            var Coupon = await _appDb.Coupons.Where(x => x.Code == dto.Code).FirstOrDefaultAsync();
            if (Coupon != null)
                return new ApiResponse<CreateCouponDto>
                    (
                   Success: false,
                   Message: "The code is found , change the code",
                   Data: null,
                   Errors: new[] { "Change the code to added it" },
                   TraceId: Guid.NewGuid().ToString()
                   );
            var CouponDto = new CreateCouponDto
            {
                Code = dto.Code,
                Description = dto.Description,
                DiscountType = dto.DiscountType,
                DiscountValue = dto.DiscountValue,
                MinPurchaseAmount = dto.MinPurchaseAmount,
                MaxDiscountAmount = dto.MaxDiscountAmount,
                StartDate = DateTime.UtcNow,
                EndDate = dto.EndDate,
                UsageLimit = dto.UsageLimit,
                UsageLimitPerUser = dto.UsageLimitPerUser,
                IsActive = dto.IsActive
            };
            var CouponEntity= new Coupon
            {
                Code = CouponDto.Code,
                Description = CouponDto.Description,
                DiscountType = CouponDto.DiscountType,
                DiscountValue = CouponDto.DiscountValue,
                MinPurchaseAmount = CouponDto.MinPurchaseAmount,
                MaxDiscountAmount = CouponDto.MaxDiscountAmount,
                StartDate = CouponDto.StartDate,
                EndDate = CouponDto.EndDate,
                UsageLimit = CouponDto.UsageLimit,
                UsageLimitPerUser = CouponDto.UsageLimitPerUser,
                IsActive = CouponDto.IsActive
            };
            await _appDb.Coupons.AddAsync(CouponEntity);
            await _appDb.SaveChangesAsync();
            return new ApiResponse<CreateCouponDto>
                (
               Success: true,
               Message: "The coupon is added successfully",
               Data: CouponDto,
               Errors: null,
               TraceId: Guid.NewGuid().ToString()
               );
        }
        public Task<ApiResponse<bool>> DeleteCouponAsync(int couponId)
        {
            var coupon =  _appDb.Coupons.Find(couponId);
            if (coupon == null)
            {
                return Task.FromResult(new ApiResponse<bool>
                (
                    Success: false,
                    Message: "Coupon not found",
                    Data: false,
                    Errors: new[] { "The specified coupon does not exist." },
                    TraceId: Guid.NewGuid().ToString()
                ));
            }
            _appDb.Coupons.Remove(coupon);
            _appDb.SaveChanges();
            return Task.FromResult(new ApiResponse<bool>
            (
                Success: true,
                Message: "Coupon deleted successfully",
                Data: true,
                Errors: null,
                TraceId: Guid.NewGuid().ToString()
            ));
        }

        public async Task<ApiResponse<List<CouponDto>>> GetAllCouponsAsync(CouponFilterDto dto)
        {
            var query = _appDb.Coupons.AsNoTracking().AsQueryable();
            if (dto.IsActive.HasValue)
            {
                query = query.Where(c => c.IsActive == dto.IsActive.Value);
            }
            var totalItems = await query.CountAsync();
            if(!string.IsNullOrWhiteSpace(dto.Search))
            {
                var search = await query.Where(x => EF.Functions.Like(x.Code, $"%{dto.Search}%"))
                    .Select(c => new CouponDto
                    {
                        Id = c.Id,
                        Code = c.Code,
                        Description = c.Description,
                        DiscountType = c.DiscountType,
                        DiscountValue = c.DiscountValue,
                        MinPurchaseAmount = c.MinPurchaseAmount,
                        MaxDiscountAmount = c.MaxDiscountAmount,
                        StartDate = c.StartDate,
                        EndDate = c.EndDate,
                        UsageLimit = c.UsageLimit,
                        UsageLimitPerUser = c.UsageLimitPerUser,
                        IsActive = c.IsActive
                    }).ToListAsync();
                return new ApiResponse<List<CouponDto>>
             (
                 Success: true,
                 Message: "Coupons retrieved successfully",
                 Data: search,
                 Errors: null,
                 TraceId: Guid.NewGuid().ToString()
             );
            }
            var coupons = await query
                .Skip((dto.Page - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .Select(c => new CouponDto
                {
                    Id = c.Id,
                    Code = c.Code,
                    Description = c.Description,
                    DiscountType = c.DiscountType,
                    DiscountValue = c.DiscountValue,
                    MinPurchaseAmount = c.MinPurchaseAmount,
                    MaxDiscountAmount = c.MaxDiscountAmount,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    UsageLimit = c.UsageLimit,
                    UsageLimitPerUser = c.UsageLimitPerUser,
                    IsActive = c.IsActive
                })
                .ToListAsync();

            return new ApiResponse<List<CouponDto>>
            (
                Success: true,
                Message: "Coupons retrieved successfully",
                Data: coupons,
                Errors: null,
                TraceId: Guid.NewGuid().ToString()
            );
        }

        public async Task<CouponDto> GetCouponByIdAsync(int couponId)
        {
            var coupon =  _appDb.Coupons
                .Where(c => c.Id == couponId)
                .Select(c => new CouponDto
                {
                    Id = c.Id,
                    Code = c.Code,
                    Description = c.Description,
                    DiscountType = c.DiscountType,
                    DiscountValue = c.DiscountValue,
                    MinPurchaseAmount = c.MinPurchaseAmount,
                    MaxDiscountAmount = c.MaxDiscountAmount,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    UsageLimit = c.UsageLimit,
                    UsageLimitPerUser = c.UsageLimitPerUser,
                    IsActive = c.IsActive
                })
                .FirstOrDefault();
            return coupon;
        }

        public async Task<ApiResponse<CouponDto>> GetCouponsAsync(string couponCode)
        {
            var coupon =  _appDb.Coupons
                .Where(c => c.Code == couponCode)
                .Select(c => new CouponDto
                {
                    Id = c.Id,
                    Code = c.Code,
                    Description = c.Description,
                    DiscountType = c.DiscountType,
                    DiscountValue = c.DiscountValue,
                    MinPurchaseAmount = c.MinPurchaseAmount,
                    MaxDiscountAmount = c.MaxDiscountAmount,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    UsageLimit = c.UsageLimit,
                    UsageLimitPerUser = c.UsageLimitPerUser,
                    IsActive = c.IsActive
                })
                .FirstOrDefault();

            return new ApiResponse<CouponDto>
            (
                Success: coupon != null,
                Message: coupon != null ? "Coupon retrieved successfully" : "Coupon not found",
                Data: coupon,
                Errors: coupon != null ? null : new[] { "The specified coupon does not exist." },
                TraceId: Guid.NewGuid().ToString()
            );
        }

        public async Task<ApiResponse<List<CouponStatsDto>>> GetCouponStatsAsync(CouponFilterDto filterDto)
        {
            var coupons =  await _appDb.CouponUsages.Include(x => x.User)
                .Where(x=>EF.Functions.Like(x.Coupon.Code,$"%{filterDto.Search}%"))
                .Select(x => new CouponStatsDto
                {
                    CouponId = x.Id,
                    CouponCode = x.Coupon.Code,
                    TotalUsage = _appDb.CouponUsages.Count(cu => cu.CouponId == x.Id),
                    TotalDiscountGiven = _appDb.CouponUsages.Count(cu => cu.CouponId == x.Id && cu.DiscountAmount > 0),
                    RecentUsage = _appDb.CouponUsages.Select(x => new CouponUsageDetailDto
                    {
                        UserEmail = x.User.Email,
                        DiscountAmount = x.DiscountAmount,
                        UsedAt = x.UsedAt
                    }).ToList(),
                }).ToListAsync();
            return new ApiResponse<List<CouponStatsDto>>
                (
                Success: coupons != null ? true : false,
                Message:coupons !=null ? "Couposn State is Retrived Success" :"not found coupons",
                Data: coupons,
                Errors:coupons!=null ? null : new[] {$"{coupons}"},
                TraceId:Guid.NewGuid().ToString()
                );
        }

        public async Task<ValidateCouponResponseDto> IsCouponValidAsync(ValidateCouponDto validateCouponDto)
        {
            var Code = await _appDb.Coupons.Where(x => x.Code == validateCouponDto.Code).FirstOrDefaultAsync();
            if (Code == null)
                return new ValidateCouponResponseDto{Message="Coupon not found"};

            if (!Code.IsActive)
                return new ValidateCouponResponseDto { Message = "Coupon is not active" };

            if (Code.EndDate < DateTime.UtcNow)
                return new ValidateCouponResponseDto { Message = "Coupon expired" };

            if (Code.UsageCount >= Code.UsageLimit)
                return new ValidateCouponResponseDto { Message = "Coupon usage limit reached" };

                return new ValidateCouponResponseDto
                {
                    Message = "The code is Valide",
                    CouponCode=validateCouponDto.Code,
                    DiscountAmount=Code.MaxDiscountAmount,
                    DiscountType=Code.DiscountType,
                    DiscountValue=Code.DiscountValue,
                    IsValid=true,
                };

        }

        public async Task <CouponDto> UpdateCouponAsync(int couponId, UpdateCouponDto updateCouponDto)
        {
            var Code= await _appDb.Coupons.Where(x=>x.Id==couponId).FirstOrDefaultAsync();
            if (Code == null)
                throw new Exception("not found Coupon");
            if (!string.IsNullOrWhiteSpace(updateCouponDto.Description))
            {
              Code.Description = updateCouponDto.Description;
            }

            if (updateCouponDto.DiscountValue.HasValue)
            {
                Code.DiscountValue = updateCouponDto.DiscountValue.Value;
            }

            if (updateCouponDto.MinPurchaseAmount.HasValue)
            {
                Code.MinPurchaseAmount = updateCouponDto.MinPurchaseAmount.Value;
            }

            if (updateCouponDto.MaxDiscountAmount.HasValue)
            {
                Code.MaxDiscountAmount = updateCouponDto.MaxDiscountAmount.Value;
            }

            if (updateCouponDto.StartDate.HasValue)
            {
                Code.StartDate = updateCouponDto.StartDate.Value;
            }

            if (updateCouponDto.EndDate.HasValue)
            {
                Code.EndDate = updateCouponDto.EndDate.Value;
            }

            if (updateCouponDto.UsageLimit.HasValue)
            {
                Code.UsageLimit = updateCouponDto.UsageLimit.Value;
            }

            if (updateCouponDto.UsageLimitPerUser.HasValue)
            {
                Code.UsageLimitPerUser = updateCouponDto.UsageLimitPerUser.Value;
            }

            if (updateCouponDto.IsActive.HasValue)
            {
                Code.IsActive = updateCouponDto.IsActive.Value;
            }
            await _appDb.SaveChangesAsync();
            var CodeDto= await _appDb.Coupons.Select(c => new CouponDto
            {
                Id = c.Id,
                Code = c.Code,
                Description = c.Description,
                DiscountType = c.DiscountType,
                DiscountValue = c.DiscountValue,
                MinPurchaseAmount = c.MinPurchaseAmount,
                MaxDiscountAmount = c.MaxDiscountAmount,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                UsageLimit = c.UsageLimit,
                UsageLimitPerUser = c.UsageLimitPerUser,
                IsActive = c.IsActive
            }).FirstOrDefaultAsync();

            return CodeDto;
        }
    }
}
