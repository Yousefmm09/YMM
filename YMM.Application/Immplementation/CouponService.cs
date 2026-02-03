using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Abstract.Repositories;
using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Cart;
using YMM.Application.Dto.Coupon;
using YMM.Application.Dto.Response;

namespace YMM.Application.Immplementation
{
    public class CouponService : ICouponsService
    {
        private readonly ICouponsRepo _coupons;
        private readonly IHttpContextAccessor _http;
        public CouponService(ICouponsRepo coupons,IHttpContextAccessor httpContext)
        {
            _coupons = coupons;
            _http = httpContext;
        }
        public async Task<ApiResponse<string>> ApplyCouponToOrderAsync(ApplyCouponDto dto,int cartId)
        {
            var user = _http.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var res= await _coupons.ApplyCouponToOrderAsync(dto, user,cartId);
            return res;
        }

        public async Task<ApiResponse<CreateCouponDto>> CreatCouponsAsync(CreateCouponDto createCouponDto)
        {
            var Coupon= await _coupons.CreatCouponsAsync(createCouponDto);
            return Coupon;
        }

        public Task<ApiResponse<bool>> DeleteCouponAsync(int couponId)
        {
            var res=_coupons.DeleteCouponAsync(couponId);
            return res;
        }

        public async Task<ApiResponse<List<CouponDto>>> GetAllCouponsAsync(CouponFilterDto dto)
        {
            var res=await _coupons.GetAllCouponsAsync(dto);
            return res;
        }

        public async Task<CouponDto> GetCouponByIdAsync(int couponId)
        {
            var res= await _coupons.GetCouponByIdAsync(couponId);
            return res;
        }

        public async Task<ApiResponse<CouponDto>> GetCouponsAsync(string couponCode)
        {
            var res= await _coupons.GetCouponsAsync(couponCode);
            return res;
        }

        public Task<ApiResponse<List<CouponStatsDto>>> GetCouponStatsAsync(CouponFilterDto filterDto)
        {
            var res=_coupons.GetCouponStatsAsync(filterDto);
            return res;
        }

        public async Task<ValidateCouponResponseDto> IsCouponValidAsync(ValidateCouponDto validateCouponDto)
        {
            var res=await _coupons.IsCouponValidAsync(validateCouponDto);
            return res;
        }

        public async Task<CouponDto> UpdateCouponAsync(int couponId, UpdateCouponDto updateCouponDto)
        {
            var res = await _coupons.UpdateCouponAsync(couponId, updateCouponDto);
            return res;
        }
    }
}
