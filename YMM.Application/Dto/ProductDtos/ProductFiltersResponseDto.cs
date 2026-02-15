using System.Collections.Generic;

namespace YMM.Application.Dto.ProductDtos
{
    public class ProductFiltersResponseDto
    {
        public string ProductName { get; set; }
        public List<string> Sizes { get; set; } = new List<string>();
        public List<string> Colors { get; set; } = new List<string>();
        public string SingleColor { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public List<BrandFilterDto> Brands { get; set; } = new List<BrandFilterDto>();
        public List<CategoryFilterDto> Categories { get; set; } = new List<CategoryFilterDto>();
    }

    public class BrandFilterDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int ProductCount { get; set; }
    }

    public class CategoryFilterDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int ProductCount { get; set; }
    }
}
