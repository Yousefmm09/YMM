using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Dto.Response;
using YMM.Application.Dto.Wishlist;

namespace YMM.Application.Abstract.Services
{
    public interface IWishListService
    {
        Task<ApiResponse<WishlistItemDto>> AddToWishList(AddToWishlistDto dto);
        Task<string> RemovWishList(int ProductId);
        Task<ApiResponse<List<WishlistItemDto>>> GetWishList();
        Task<ApiResponse<string>> AddtoCartItem(int wislistId);
        //clear 
        Task<string> ClearWishList();
    }
}
