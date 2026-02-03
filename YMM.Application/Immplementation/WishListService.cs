using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Abstract.Repositories;
using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Response;
using YMM.Application.Dto.Wishlist;

namespace YMM.Application.Immplementation
{
    public class WishListService:IWishListService
    {
        private readonly IWishList _wishListRepository;  
        private readonly IHttpContextAccessor _httpContextAccessor;
        public WishListService(IWishList wishListRepository, IHttpContextAccessor httpContextAccessor)
        {
            _wishListRepository = wishListRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public  async Task<ApiResponse<WishlistItemDto>> AddToWishList(AddToWishlistDto dto)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var res= await _wishListRepository.AddToWishList(dto, userId);
            return res;
        }

        public Task<string> RemovWishList(int ProductId)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return _wishListRepository.RemovWishList(ProductId, userId);
        }

        public Task<ApiResponse<List<WishlistItemDto>>> GetWishList()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return _wishListRepository.GetWishList(userId);
        }

        public Task<ApiResponse<string>> AddtoCartItem(int wislistId)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return _wishListRepository.AddtoCartItem(wislistId, userId);
        }

        public Task<string> ClearWishList()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return _wishListRepository.ClearWishList(userId);
        }
    }
}
