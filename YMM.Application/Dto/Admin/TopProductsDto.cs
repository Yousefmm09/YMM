using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YMM.Application.Dto.Admin
{
    // ========================================
    // REQUEST DTO
    // ========================================

    public class TopProductsRequestDto
    {
        public int Limit { get; set; } = 10;

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        [MaxLength(20)]
        public string Period { get; set; } = "30days"; // 7days, 30days, 90days, year, alltime

        [MaxLength(20)]
        public string SortBy { get; set; } = "revenue"; // revenue, quantity, profit, views

        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
    }

    // ========================================
    // RESPONSE DTO
    // ========================================

    public class TopProductsDto
    {
        // ========================================
        // TOP PRODUCTS LIST
        // ========================================
        public List<TopProductItemDto> Products { get; set; } = new List<TopProductItemDto>();

        // ========================================
        // SUMMARY
        // ========================================
        public TopProductsSummaryDto Summary { get; set; } = null!;
    }

    public class TopProductItemDto
    {
        public int Rank { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string ProductSlug { get; set; } = null!;
        public string SKU { get; set; } = null!;
        public string? ImageUrl { get; set; }

        public string CategoryName { get; set; } = null!;
        public string BrandName { get; set; } = null!;

        // ========================================
        // SALES METRICS
        // ========================================
        public int QuantitySold { get; set; }
        public decimal Revenue { get; set; }
        public decimal Cost { get; set; }
        public decimal Profit { get; set; }
        public decimal ProfitMargin { get; set; } // Percentage

        public int OrdersCount { get; set; }
        public decimal AverageOrderQuantity { get; set; }

        // ========================================
        // PERFORMANCE
        // ========================================
        public int ViewCount { get; set; }
        public decimal ConversionRate { get; set; } // (Orders / Views) * 100

        public int CurrentStock { get; set; }
        public int StockValue { get; set; }

        // ========================================
        // TREND
        // ========================================
        public decimal SalesGrowth { get; set; } // Compared to previous period
        public string Trend { get; set; } = null!; // "up", "down", "stable"
    }

    public class TopProductsSummaryDto
    {
        public decimal TotalRevenue { get; set; }
        public int TotalQuantitySold { get; set; }
        public int TotalProducts { get; set; }
        public decimal AverageRevenuePerProduct { get; set; }
    }

    // ========================================
    // LEGACY SUPPORT (for backward compatibility)
    // ========================================

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
