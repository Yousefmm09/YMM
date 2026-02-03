using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Cart;
using YMM.Application.Dto.Coupon;
using YMM.Application.Immplementation;

namespace YMM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly ICouponsService _couponsService;
        public CouponController(ICouponsService coupons)
        {
            _couponsService = coupons;
        }
        [HttpPost("Creat")]
        public  async Task<IActionResult> CreatCoupons(CreateCouponDto createCouponDto)
        {
            var result =await _couponsService.CreatCouponsAsync(createCouponDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpDelete("Delete/{CouponId}")]
        public  async Task<IActionResult> DeleteCoupon([FromRoute]int CouponId)
        {
            var result =await _couponsService.DeleteCouponAsync(CouponId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("GetCoupons/{code}")]
        public  async Task<IActionResult> GetCoupons([FromRoute]string code)
        {
            var result =await _couponsService.GetCouponsAsync(code);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("GetAllCouponsAsync")]
        public  async Task<IActionResult> GetAllCouponsAsync([FromQuery] CouponFilterDto dto)
        {
            var result =await _couponsService.GetAllCouponsAsync(dto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("GetCouponStats")]
        public  async Task<IActionResult> GetCouponStatsAsync([FromQuery] CouponFilterDto dto)
        {
            var result =await _couponsService.GetCouponStatsAsync(dto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("ApplyCouponToCart")]
        public  async Task<IActionResult> ApplyCouponToCart([FromQuery] ApplyCouponDto dto, int cartId )
        {
            var result =await _couponsService.ApplyCouponToOrderAsync(dto,cartId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("IsCouponValid")]
        public  async Task<IActionResult> IsCouponValidAsync([FromQuery] ValidateCouponDto dto )
        {
            var result =await _couponsService.IsCouponValidAsync(dto);
            if (!result.IsValid)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPatch("UpdateCoupon/{CodeId}")]
        public  async Task<IActionResult> UpdateCouponAsync([FromRoute] int CodeId,[FromForm]UpdateCouponDto dto )
        {
            var result =await _couponsService.UpdateCouponAsync(CodeId,dto);
            if (!result.IsValid)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

    }
}
