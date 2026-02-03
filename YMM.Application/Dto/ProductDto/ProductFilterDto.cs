using YMM.Application.Dto.Common;

namespace YMM.Application.Dto.Product
{
    public class ProductFilterDto :PaginationParams
    {
        public string? Search { get; set; }
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public string? SortBy { get; set; } // price, name, createdAt, rating
        public string? SortOrder { get; set; } = "asc"; // asc, desc
        public bool? InStock { get; set; }
        public bool? IsFeatured { get; set; }
        public bool? IsNew { get; set; }
        public bool? OnSale { get; set; }
    }
}
