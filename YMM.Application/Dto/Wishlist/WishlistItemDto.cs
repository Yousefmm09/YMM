using System;

namespace YMM.Application.Dto.Wishlist
{
    public class WishlistItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string ProductSlug { get; set; } = null!;
        public string? ProductImage { get; set; }
        public decimal Price { get; set; }
        public decimal? SalePrice { get; set; }
        public bool InStock { get; set; }
        public string BrandName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
