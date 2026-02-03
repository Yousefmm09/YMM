using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Encodings;
using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Cart;

namespace YMM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [HttpGet("GetCart")]
        public async Task<IActionResult> GetCart()
        {
            var result = await _cartService.GetCartAsync();
            if (!result.Success)
            {
                return Unauthorized(result);
            }
            return Ok(result);
        }
        [HttpGet("GetCartById/{cartId}")]
        public async Task<IActionResult> GetCartById(int cartId)
        {
            var result = await _cartService.GetCartByIdAsync(cartId);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto dto)
        {
            var result = await _cartService.AddToCartAsync(dto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPut("UpdateCartItem/{cartItemId}")]
        public async Task<IActionResult> UpdateCartItem(int cartItemId, [FromBody] UpdateCartItemDto dto)
        {
            var result = await _cartService.UpdateCartItemAsync(cartItemId, dto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpDelete("RemoveFromCart/{cartItemId}")]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            var result = await _cartService.RemoveFromCartAsync(cartItemId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("HealthCheck")]
        public IActionResult HealthCheck()
        {
            return Ok("CartController is healthy.");
        }
    }
}
