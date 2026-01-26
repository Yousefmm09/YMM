using System;
using System.Collections.Generic;

namespace YMM.Application.Dto.Product
{
    public class ProductDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public string? SKU { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string CategorySlug { get; set; } = null!;
        public int BrandId { get; set; }
        public string BrandName { get; set; } = null!;
        public string BrandSlug { get; set; } = null!;
        public bool IsFeatured { get; set; }
        public bool IsNew { get; set; }
        public bool IsActive { get; set; }
        public bool InStock { get; set; }
        public int TotalStock { get; set; }
        public decimal? AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public int ViewCount { get; set; }
        public int SoldCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<ProductVariantDto> Variants { get; set; } = new List<ProductVariantDto>();
        public List<ProductImageDto> Images { get; set; } = new List<ProductImageDto>();
        public List<string> AvailableSizes { get; set; } = new List<string>();
        public List<string> AvailableColors { get; set; } = new List<string>();
    }
}
