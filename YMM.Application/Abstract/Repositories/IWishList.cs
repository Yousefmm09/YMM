using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Dto.Response;
using YMM.Application.Dto.Wishlist;

namespace YMM.Application.Abstract.Repositories
{
    public interface IWishList
    {
        Task<ApiResponse<WishlistItemDto>> AddToWishList(AddToWishlistDto dto,string userId);
        Task<string>RemovWishList(int ProductId,string userId);
        Task<ApiResponse<List<WishlistItemDto>>> GetWishList(string userId);
        Task<ApiResponse<string>>AddtoCartItem(int wislistId,string userId);
        //clear 
        Task<string> ClearWishList(string userId);
    }
}
