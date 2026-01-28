using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Dto.Cart;
using YMM.Application.Dto.Response;

namespace YMM.Application.Abstract.Repositories
{
    public interface ICartRepo
    {
        Task<CartDto> GetCartAsync(string userId);
        public  Task<CartDto> GetCartByIdAsync(int cartId);
        Task<ApiResponse<CartDto>> AddToCartAsync(string userId, AddToCartDto dto);
        Task<ApiResponse<CartDto>> UpdateCartItemAsync(string userId, int cartItemId, UpdateCartItemDto dto);
        Task<ApiResponse<CartDto>> RemoveFromCartAsync(string userId, int cartItemId);
    }
}
