using System;

namespace YMM.Application.Dto.Product
{
    public class ProductListDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public string? MainImageUrl { get; set; }
        public string CategoryName { get; set; } = null!;
        public string BrandName { get; set; } = null!;
        public bool IsFeatured { get; set; }
        public bool IsNew { get; set; }
        public bool InStock { get; set; }
        public decimal? AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
