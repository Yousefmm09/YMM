namespace YMM.Application.Dto.ProductDtos
{
    public class ProductSearchSuggestionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public string BrandName { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
    }
}
