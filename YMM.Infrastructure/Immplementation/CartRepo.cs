using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Abstract.Repositories;
using YMM.Application.Dto.Cart;
using YMM.Application.Dto.Response;
using YMM.Data.Entities;
using YMM.Data.Entities.Identity;
using YMM.Infrastructure.Context;

namespace YMM.Infrastructure.Immplementation
{
    public class CartRepo:ICartRepo
    {
        private readonly AppDb _appDb;
        public CartRepo(AppDb appDb)
        {
         _appDb = appDb;   
        }

        public async Task<ApiResponse<CartDto>> AddToCartAsync(string userId, AddToCartDto dto)
        {
            using var transaction = await _appDb.Database.BeginTransactionAsync();
            var product = await _appDb.Products
                .Include(p => p.Variants)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == dto.ProductId && p.IsActive && !p.IsDeleted);

            if (product == null)
            {
                return new ApiResponse<CartDto>
                (
                    Success: false,
                    Message: "Product not found",
                    Data: null,
                    Errors: new[] { "Product does not exist or is not available" },
                    TraceId: Guid.NewGuid().ToString()
                );
            }

            ProductVariant variant = null;
            if (dto.VariantId.HasValue)
            {
                variant =  product.Variants.FirstOrDefault(v => v.Id == dto.VariantId.Value);
                if (variant == null)
                {
                    return new ApiResponse<CartDto>
                    (
                        Success: false,
                        Message: "Variant not found",
                        Data: null,
                        Errors: new[] { "Product variant does not exist" },
                        TraceId: Guid.NewGuid().ToString()
                    );
                }

            }
            if(variant.StockQuantity< dto.Quantity)
            {
                return new ApiResponse<CartDto>
                   (
                       Success: false,
                       Message: "Insufficient stock",
                       Data: null,
                       Errors: new[] { $"Only {variant.StockQuantity} items available" },
                       TraceId: Guid.NewGuid().ToString()
                   );
            }
            var cart = await GetCartAsync(userId);
            var existingItem =  await _appDb.CartItems.FirstOrDefaultAsync(x => x.ProductId == dto.ProductId
            && x.Variant.Id == dto.VariantId && x.CartId == cart.Id);
            if(existingItem != null)
            {
                var newQuantity = existingItem.Quantity + dto.Quantity; // 0+2=2
                variant.StockQuantity = variant.StockQuantity - dto.Quantity; // 10-2=8 
                var availableStock = variant?.StockQuantity ?? int.MaxValue; // 8
                if (newQuantity > availableStock)
                {
                    return new ApiResponse<CartDto>
                    (
                        Success: false,
                        Message: "Insufficient stock",
                        Data: null,
                        Errors: new[] { $"Cannot add {dto.Quantity} more items. Only {availableStock - existingItem.Quantity} available" },
                        TraceId: Guid.NewGuid().ToString()
                    );
                }
                existingItem.Quantity= newQuantity;
                existingItem.Total = existingItem.Price * existingItem.Quantity;
            }
            else
            {
                
                    var price = product.SalePrice ?? product.Price;
                if (variant != null && variant.PriceAdjustment.HasValue)
                {
                    price += variant.PriceAdjustment.Value;
                    variant.StockQuantity = variant.StockQuantity - dto.Quantity;
                }
                var cartItem = new CartItem
                    {
                        CartId = cart.Id,
                        ProductId = dto.ProductId,
                        VariantId = dto.VariantId,
                        Quantity = dto.Quantity,
                        Price = price,
                        Total = price * dto.Quantity,
                        CreatedAt = DateTime.UtcNow
                    };  
                await _appDb.CartItems.AddAsync(cartItem);
                await _appDb.SaveChangesAsync();
            }
            await RecalculateCartTotalsAsync(cart.Id);
            await _appDb.SaveChangesAsync();
           await transaction.CommitAsync();


            var updatedCart = await GetCartByIdAsync(cart.Id);

            return new ApiResponse<CartDto>
            (
                Success: true,
                Message: "Item added to cart successfully",
                Data: updatedCart,
                Errors: null,
                TraceId: Guid.NewGuid().ToString()
            );
        }
        private async Task RecalculateCartTotalsAsync(int cartId)
        {
            var cart = await _appDb.Carts
                .Include(c => c.Items)
                .Include(c => c.Coupon)
                .FirstOrDefaultAsync(c => c.Id == cartId);

            if (cart == null)
                return;
            cart.Subtotal = cart.Items.Sum(i => i.Total);
            cart.Discount = 0;

            if (cart.Total < 0)
                cart.Total = 0;


            cart.UpdatedAt = DateTime.UtcNow;
            cart.Total = cart.Subtotal - cart.Discount + cart.Tax;
            _appDb.Carts.Update(cart);
        }
        public async Task<CartDto> GetCartByIdAsync(int cartId)
        {
            var cart = await _appDb.Carts.Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Variants)
                .Include(x => x.Coupon)
                .FirstOrDefaultAsync(x => x.Id == cartId);
            if (cart == null)
                return null;
            var cartdto = new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                Items = cart.Items.Select(item => new CartItemDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    ProductSlag = item.Product.Slug,
                    ProductImage = item.Product.Images.FirstOrDefault(i => i.IsMain)?.ImageUrl,
                    Size= item.Variant?.Size,
                    Color= item.Variant?.Color,
                    VariantId = item.VariantId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    Total = item.Total,
                    AvailableStock = item.Variant?.StockQuantity ?? 999

                }).ToList(),
                Subtotal = cart.Subtotal,
                Discount = cart.Discount,
                Tax = cart.Tax,
                Total = cart.Total,
                CouponCode = cart.Coupon?.Code,
                ItemsCount = cart.Items.Sum(i => i.Id),
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt,
            };
            return cartdto;
        }
        public async Task<CartDto> GetCartAsync(string userId)
        {
            var cart =   await _appDb.Carts.Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Variants)
                .Include(x => x.Coupon)
                .FirstOrDefaultAsync(x=>x.UserId== userId);
            if (cart==null) 
                return null;
            var cartdto = new CartDto
            {
                Id = cart.Id,
                UserId = userId,
                Items = cart.Items.Select(item => new CartItemDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    ProductSlag = item.Product.Slug,
                    ProductImage = item.Product.Images.FirstOrDefault(i => i.IsMain)?.ImageUrl,
                    VariantId = item.VariantId,
                    Size = item.Variant?.Size,
                    Color = item.Variant?.Color,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    Total = item.Total,
                    AvailableStock = item.Variant?.StockQuantity ?? 999
                }).ToList(),
                Subtotal = cart.Subtotal,
                Discount = cart.Discount,
                Tax = cart.Tax,
                Total = cart.Total,
                CouponCode = cart.Coupon?.Code,
                ItemsCount = cart.Items.Sum(i => i.Id),
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt,
            };
            return cartdto;
        }

        public async Task<ApiResponse<CartDto>> RemoveFromCartAsync(string userId, int cartItemId)
        {
            var cartItem =  _appDb.CartItems.Include(x => x.Cart).Include(x=>x.Variant)
                .Include(x => x.Product).FirstOrDefault(x => x.Id == cartItemId);
            var variant = await _appDb.ProductVariants.AsNoTracking().FirstOrDefaultAsync(x => x.Id == cartItem.VariantId);
            if (cartItem == null)
                return new ApiResponse<CartDto>
               (
                   Success: false,
                   Message: "Cart item not found",
                   Data: null,
                   Errors: new[] { "Item does not exist in your cart" },
                   TraceId: Guid.NewGuid().ToString()
                );
            if (cartItem.Cart.UserId != userId)
                return new ApiResponse<CartDto>
               (
                   Success: false,
                   Message: "Unauthorized",
                   Data: null,
                   Errors: new[] { "You are not authorized to remove this item" },
                   TraceId: Guid.NewGuid().ToString()
                );
            // decrease the subtotal, total, items count
            foreach (var item in cartItem.Cart.Items)
            {
                if (item.Id == cartItemId)
                {
                    cartItem.Cart.Subtotal -= item.Total;
                    cartItem.Cart.Total -= item.Total;
                    break;
                }
            }
            variant.StockQuantity += cartItem.Quantity;
            _appDb.CartItems.Remove(cartItem);
            await _appDb.SaveChangesAsync();
            var updatedCart = await GetCartAsync(userId);
            return new ApiResponse<CartDto>
               (
                   Success: true,
                   Message: "Item removed from cart successfully",
                   Data: updatedCart,
                   Errors: null,
                   TraceId: Guid.NewGuid().ToString()
                );
        }

        public async Task<ApiResponse<CartDto>> UpdateCartItemAsync( string userId, int cartItemId, UpdateCartItemDto dto)
        {
            using var transaction = await _appDb.Database.BeginTransactionAsync();

                var cartItem = await _appDb.CartItems
                    .Include(ci => ci.Cart)
                        .ThenInclude(c => c.Items) 
                    .Include(ci => ci.Product)
                    .Include(ci => ci.Variant)
                    .FirstOrDefaultAsync(ci => ci.Id == cartItemId);

                if (cartItem == null)
                {
                    return new ApiResponse<CartDto>
                    (
                        Success: false,
                        Message: "Cart item not found",
                        Data: null,
                        Errors: new[] { "Item does not exist in your cart" },
                        TraceId: Guid.NewGuid().ToString()
                    );
                }

                if (cartItem.Cart.UserId != userId)
                {
                    return new ApiResponse<CartDto>
                    (
                        Success: false,
                        Message: "Unauthorized",
                        Data: null,
                        Errors: new[] { "You are not authorized to update this item" },
                        TraceId: Guid.NewGuid().ToString()
                    );
                }

              
                if (dto.Quantity <= 0)
                {
                    return new ApiResponse<CartDto>
                    (
                        Success: false,
                        Message: "Invalid quantity",
                        Data: null,
                        Errors: new[] { "Quantity must be greater than 0" },
                        TraceId: Guid.NewGuid().ToString()
                    );
                }

                if (cartItem.Variant != null)
                {
                    if (cartItem.Variant.StockQuantity < dto.Quantity)
                    {
                        return new ApiResponse<CartDto>
                        (
                            Success: false,
                            Message: "Insufficient stock",
                            Data: null,
                            Errors: new[] {
                        $"Only {cartItem.Variant.StockQuantity} items available. " +
                        $"You are trying to update to {dto.Quantity}"
                            },
                            TraceId: Guid.NewGuid().ToString()
                        );
                    }
                }

                
            cartItem.Total = cartItem.Price * dto.Quantity;
            // Adjust stock quantity
            if (cartItem.Variant != null)
            {
                if (dto.Quantity < cartItem.Quantity) 
                {
                    cartItem.Variant.StockQuantity = cartItem.Variant.StockQuantity + (cartItem.Quantity - dto.Quantity);
                }
                else
                {
                    cartItem.Variant.StockQuantity = cartItem.Variant.StockQuantity - (dto.Quantity);
                }
                //cartItem.Variant.StockQuantity = cartItem.Variant.StockQuantity - dto.Quantity;
                _appDb.ProductVariants.Update(cartItem.Variant);
            }
            cartItem.Quantity = dto.Quantity;
            _appDb.CartItems.Update(cartItem);

                await RecalculateCartTotalsAsync(cartItem.CartId);

                await _appDb.SaveChangesAsync();
                await transaction.CommitAsync();

                
                var updatedCart = await GetCartByIdAsync(cartItem.CartId);

                return new ApiResponse<CartDto>
                (
                    Success: true,
                    Message: "Cart item updated successfully",
                    Data: updatedCart,
                    Errors: null,
                    TraceId: Guid.NewGuid().ToString()
                );
            
          
                await transaction.RollbackAsync();
            

                return new ApiResponse<CartDto>
                (
                    Success: false,
                    Message: "An error occurred while updating cart item",
                    Data: null,
                    Errors: new { Message="" },
                    TraceId: Guid.NewGuid().ToString()
                );
            
        }
    }
}
