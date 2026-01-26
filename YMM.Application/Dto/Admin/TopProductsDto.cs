namespace YMM.Application.Dto.Admin
{
    public class TopProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? ProductImage { get; set; }
        public string BrandName { get; set; } = null!;
        public int TotalSold { get; set; }
        public decimal TotalRevenue { get; set; }
        public int CurrentStock { get; set; }
    }

    public class TopProductsFilterDto
    {
        public int Limit { get; set; } = 10;
        public string Period { get; set; } = "30days"; // 7days, 30days, 90days, year
    }
}
