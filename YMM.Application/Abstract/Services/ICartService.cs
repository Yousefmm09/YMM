using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Dto.Cart;
using YMM.Application.Dto.Response;

namespace YMM.Application.Abstract.Services
{
    public interface ICartService
    {
        Task<ApiResponse<CartDto>> GetCartAsync();
        Task<ApiResponse<CartDto>> GetCartByIdAsync(int cartId);
        Task<ApiResponse<CartDto>> AddToCartAsync(AddToCartDto dto);
        Task<ApiResponse<CartDto>> UpdateCartItemAsync(int cartItemId, UpdateCartItemDto dto);
        Task<ApiResponse<CartDto>> RemoveFromCartAsync(int cartItemId);
    }
}
