using System.Collections.Generic;

namespace YMM.Application.Dto.Admin
{
    public class InventoryReportDto
    {
        public int TotalProducts { get; set; }
        public int TotalVariants { get; set; }
        public int InStockProducts { get; set; }
        public int LowStockProducts { get; set; }
        public int OutOfStockProducts { get; set; }
        public decimal TotalInventoryValue { get; set; }
        public List<LowStockProductDto> LowStockItems { get; set; } = new List<LowStockProductDto>();
        public List<OutOfStockProductDto> OutOfStockItems { get; set; } = new List<OutOfStockProductDto>();
    }

    public class LowStockProductDto
    {
        public int ProductId { get; set; }
        public int? VariantId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? Size { get; set; }
        public string? Color { get; set; }
        public int CurrentStock { get; set; }
        public int ReorderLevel { get; set; }
    }

    public class OutOfStockProductDto
    {
        public int ProductId { get; set; }
        public int? VariantId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? Size { get; set; }
        public string? Color { get; set; }
        public int DaysOutOfStock { get; set; }
    }

    public class InventoryReportFilterDto
    {
        public int LowStockThreshold { get; set; } = 10;
    }
}
