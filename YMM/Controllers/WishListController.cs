using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Wishlist;

namespace YMM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly IWishListService _wishListService;
        public WishListController(IWishListService wishListService)
        {
            _wishListService = wishListService;
        }
        [HttpPost("AddToWishList")]
        public async Task<IActionResult> AddToWishList([FromBody] AddToWishlistDto dto)
        {
            var result = await _wishListService.AddToWishList(dto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpDelete("RemoveFromWishList/{productId}")]
        public async Task<IActionResult> RemoveFromWishList(int productId)
        {
            var result = await _wishListService.RemovWishList(productId);
            return Ok(result);
        }
        [HttpGet("GetWishList")]
        public async Task<IActionResult> GetWishList()
        {
            var result = await _wishListService.GetWishList();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("AddToCart/{wishlistId}")]
        public async Task<IActionResult> AddToCart(int wishlistId)
        {
            var result = await _wishListService.AddtoCartItem(wishlistId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpDelete("ClearWishList")]
        public async Task<IActionResult> ClearWishList()
        {
            var result = await _wishListService.ClearWishList();
            return Ok(result);
        }
    }
}
