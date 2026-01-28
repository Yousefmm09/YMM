using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Abstract.Repositories;
using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Cart;
using YMM.Application.Dto.Response;

namespace YMM.Application.Immplementation
{
    public class CartService : ICartService
    {
        private readonly ICartRepo _cartRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CartService(ICartRepo cartRepo, IHttpContextAccessor httpContextAccessor)
        {
            _cartRepo = cartRepo;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ApiResponse<CartDto>> AddToCartAsync( AddToCartDto dto)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return new ApiResponse<CartDto>
                (
                    Success: false,
                    Message: "User not authenticated",
                    Data: null,
                    Errors: new { Authentication = "User must be logged in to update cart items." },
                    TraceId: null
                );
            }
            var cart= await _cartRepo.AddToCartAsync(userId,dto);
            return cart;
        }

        public async Task<ApiResponse<CartDto>> GetCartAsync()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return new ApiResponse<CartDto>
                (
                    Success: false,
                    Message: "User not authenticated",
                    Data: null,
                    Errors: new { Authentication = "User must be logged in to update cart items." },
                    TraceId: null
                );
            }
            var cart= await _cartRepo.GetCartAsync(userId);
            return new ApiResponse<CartDto>
                (
                    Success: true,
                    Message: "Cart retrieved successfully",
                    Data: cart,
                    Errors: null,
                    TraceId: null
                );
        }

        public async Task<ApiResponse<CartDto>> GetCartByIdAsync(int cartId)
        {
            var cart= await _cartRepo.GetCartByIdAsync(cartId);
            return new ApiResponse<CartDto>
                (
                    Success: cart!=null ? true : false,
                    Message: cart!=null ?"Cart retrieved successfully" : "Not found cart",
                    Data: cart,
                    Errors:cart!= null ? null : new { Message="error"},
                    TraceId: cart!= null? Guid.NewGuid().ToString():Guid.NewGuid().ToString()
                );
        }

        public async Task<ApiResponse<CartDto>> RemoveFromCartAsync(int cartItemId)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                {
                return new ApiResponse<CartDto>
                (
                    Success: false,
                    Message: "User not authenticated",
                    Data: null,
                    Errors: new { Authentication = "User must be logged in to remove items from cart." },
                    TraceId: null
                );
            }
            var cart= await _cartRepo.RemoveFromCartAsync(userId,cartItemId);
            return cart;
        }

        public async Task<ApiResponse<CartDto>> UpdateCartItemAsync( int cartItemId, UpdateCartItemDto dto)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                {
                return new ApiResponse<CartDto>
                (
                    Success: false,
                    Message: "User not authenticated",
                    Data: null,
                    Errors: new { Authentication = "User must be logged in to update cart items." },
                    TraceId: null
                );
            }
            var cart= await _cartRepo.UpdateCartItemAsync(userId,cartItemId,dto);
            return cart;
        }
    }
}
