using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Dto.Cart;
using YMM.Application.Dto.Coupon;
using YMM.Application.Dto.Response;

namespace YMM.Application.Abstract.Services
{
    public interface ICouponsService
    {
        Task<ValidateCouponResponseDto> IsCouponValidAsync(ValidateCouponDto validateCouponDto);
        Task<ApiResponse<CouponDto>> GetCouponsAsync(string couponCode);
        Task<CouponDto> GetCouponByIdAsync(int couponId);
        Task<ApiResponse<List<CouponDto>>> GetAllCouponsAsync(CouponFilterDto dto);
        Task<ApiResponse<CreateCouponDto>> CreatCouponsAsync(CreateCouponDto createCouponDto);
        Task<CouponDto> UpdateCouponAsync(int couponId, UpdateCouponDto updateCouponDto);
        Task<ApiResponse<List<CouponStatsDto>>> GetCouponStatsAsync(CouponFilterDto filterDto);
        Task<ApiResponse<string>> ApplyCouponToOrderAsync(ApplyCouponDto dto, int cartId);
        Task<ApiResponse<bool>> DeleteCouponAsync(int couponId);
    }
}
