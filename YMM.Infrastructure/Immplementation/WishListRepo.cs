using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Abstract.Repositories;
using YMM.Application.Dto.Cart;
using YMM.Application.Dto.Response;
using YMM.Application.Dto.Wishlist;
using YMM.Data.Entities;
using YMM.Data.Entities.Identity;
using YMM.Infrastructure.Context;

namespace YMM.Infrastructure.Immplementation
{
    public class WishListRepo : IWishList
    {
        private readonly AppDb _appDb;
        private readonly UserManager<User> _userManager;
        private  readonly ICartRepo _cartRepo;
        public WishListRepo(AppDb appDb,UserManager<User> userManager, ICartRepo cartRepo)
        {
            _appDb = appDb;
            _userManager = userManager;
            _cartRepo = cartRepo;
        }
        public  async Task<ApiResponse<string>> AddtoCartItem(int wislistId,string userId)
        {
            var GetUser=  await _userManager.FindByIdAsync(userId);
            if (GetUser== null)
                return new ApiResponse<string>
                    (
                   Success: false,
                   Message: "The user is not authorized or not login",
                   Data: null,
                   Errors: new[] { "Not found User , is not authorized or not regist here,please login or regist here" },
                   TraceId: Guid.NewGuid().ToString()
                   );
            var Carts=await _appDb.Carts.Where(x=>x.UserId == userId).FirstOrDefaultAsync();
            var getUserWishList=await _appDb.WishlistItems.Where(x=>x.UserId==userId && x.Id==wislistId).FirstOrDefaultAsync();
            if (getUserWishList == null)
                return new ApiResponse<string>
                    (
                   Success: false,
                   Message: "The wishlist is empty",
                   Data: null,
                   Errors: new[] { "Not found product in your wishlist" },
                   TraceId: Guid.NewGuid().ToString()
                   );
            var cartItem = new AddToCartDto
            {
                ProductId = getUserWishList.ProductId,
                Quantity = 1,
                VariantId = _appDb.Products.Where(x => x.Id == getUserWishList.ProductId).Select(x => x.Variants.FirstOrDefault().Id).FirstOrDefault(),
            };
            await _cartRepo.AddToCartAsync(GetUser.Id,cartItem);
            return new ApiResponse<string>
                    (
                   Success: true,
                   Message: "The product added to cart success",
                   Data: $"{getUserWishList.Product.Name} is added to your cart",
                   Errors: null,
                   TraceId: Guid.NewGuid().ToString()
                   );

        }

        public  async Task<ApiResponse<WishlistItemDto>> AddToWishList(AddToWishlistDto dto,string userId)
        {
            var GetUser = await _userManager.FindByIdAsync(userId);
            if (GetUser == null)
                return new ApiResponse<WishlistItemDto>
                     (
                    Success: false,
                    Message: "The user is not authorized or not login",
                    Data: null,
                    Errors: new[] { "Not found User , is not authorized or not regist here,please login or regist here" },
                    TraceId: Guid.NewGuid().ToString()
                    );
            var query = _appDb.WishlistItems.Where(x => x.UserId == GetUser.Id).AsNoTracking();
            if (query.Any(x => x.ProductId == dto.ProductId))
                throw new Exception("Not able add Same product in wishlist");

            var wishlist = new WishlistItem
                {
                    ProductId= dto.ProductId,
                    UserId=GetUser.Id,
                    CreatedAt=DateTime.Now,
                };
            var wishListDto = new WishlistItemDto
            {
                ProductId = dto.ProductId,
                Price = _appDb.Products.Where(x => x.Id == dto.ProductId).Select(x => x.Price).FirstOrDefault(),
                CreatedAt = DateTime.Now,
                SalePrice = _appDb.Products.Where(x => x.Id == dto.ProductId).Select(x => x.SalePrice).FirstOrDefault(),
                ProductName = _appDb.Products.Where(x => x.Id == dto.ProductId).Select(x => x.Name).FirstOrDefault(),
                BrandName = _appDb.Products.Where(x => x.Id == dto.ProductId).Select(x => x.Brand.Name).FirstOrDefault(),
                ProductSlug = _appDb.Products.Where(x => x.Id == dto.ProductId).Select(x => x.Slug).FirstOrDefault(),

            };
            await _appDb.WishlistItems.AddAsync(wishlist);
            await _appDb.SaveChangesAsync();
            return new ApiResponse<WishlistItemDto>
                    (
                    Success: true,
                    Message:"The product added to wishlist success",
                    Data: wishListDto,
                    Errors:null, 
                    TraceId: Guid.NewGuid().ToString()
                    );
            

        }

        public async Task<string> ClearWishList(string userId)
        {
            var query= await _appDb.WishlistItems.Where(x => x.UserId == userId).ToListAsync();
            if (query.Count() < 0)
                return "Your wishlist is empty";
              _appDb.RemoveRange(query);
             await _appDb.SaveChangesAsync();
            return "Your wishlist it clear  now";

        }

        public async Task<ApiResponse<List<WishlistItemDto>>> GetWishList(string userId)
        {
            var wishList =  await _appDb.WishlistItems.AsNoTracking().Where(x => x.UserId == userId)
                .Select(x => new WishlistItemDto
                {
                    ProductId = x.ProductId,
                    Price = x.Product.Price,
                    SalePrice = x.Product.SalePrice,
                    ProductName = x.Product.Name,
                    BrandName = x.Product.Brand.Name,
                }).ToListAsync();
            if ( wishList.Count < 0 )
                throw new Exception("not found product in your wishlist");
               return new ApiResponse<List<WishlistItemDto>>
                    (
                    Success: true,
                    Message: "The wishList Retrived success",
                    Data: wishList,
                    Errors: null,
                    TraceId: Guid.NewGuid().ToString()
                    );
        }

        public  async Task<string> RemovWishList(int Productid, string userId)
        {
            var wishList=_appDb.WishlistItems.AsNoTracking().Where(x=>x.UserId == userId && x.ProductId==Productid).FirstOrDefault();
            if (wishList == null)
                return "Not found product in your wishlist";
            _appDb.Remove(wishList);
            await _appDb.SaveChangesAsync();
            return $"{wishList.Product.Name} is removed in your wishlist";
        }
    }
}
