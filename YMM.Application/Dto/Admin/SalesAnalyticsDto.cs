using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YMM.Application.Dto.Admin
{
    // ========================================
    // REQUEST DTO
    // ========================================

    public class SalesAnalyticsRequestDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        [MaxLength(20)]
        public string Period { get; set; } = "30days"; // today, 7days, 30days, 90days, year, custom

        [MaxLength(20)]
        public string GroupBy { get; set; } = "day"; // hour, day, week, month, year
    }

    // ========================================
    // RESPONSE DTO
    // ========================================

    public class SalesAnalyticsDto
    {
        // ========================================
        // SUMMARY
        // ========================================
        public SalesSummaryDto Summary { get; set; } = null!;

        // ========================================
        // TIMELINE DATA (for Charts)
        // ========================================
        public List<SalesTimelineDto> Timeline { get; set; } = new List<SalesTimelineDto>();

        // ========================================
        // BREAKDOWN BY CATEGORY
        // ========================================
        public List<SalesByCategoryDto> ByCategory { get; set; } = new List<SalesByCategoryDto>();

        // ========================================
        // BREAKDOWN BY BRAND
        // ========================================
        public List<SalesByBrandDto> ByBrand { get; set; } = new List<SalesByBrandDto>();

        // ========================================
        // PAYMENT METHODS
        // ========================================
        public List<SalesByPaymentMethodDto> ByPaymentMethod { get; set; } = new List<SalesByPaymentMethodDto>();

        // ========================================
        // ORDER STATUS DISTRIBUTION
        // ========================================
        public List<OrderStatusDistributionDto> OrderStatusDistribution { get; set; } = new List<OrderStatusDistributionDto>();

        // ========================================
        // COMPARISON (vs Previous Period)
        // ========================================
        public SalesComparisonDto Comparison { get; set; } = null!;
    }

    // ========================================
    // SUPPORTING DTOs
    // ========================================

    public class SalesSummaryDto
    {
        public decimal TotalRevenue { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalProfit { get; set; }
        public decimal ProfitMargin { get; set; } // Percentage

        public int TotalOrders { get; set; }
        public int CompletedOrders { get; set; }
        public int CancelledOrders { get; set; }
        public int RefundedOrders { get; set; }

        public decimal AverageOrderValue { get; set; }
        public int TotalItemsSold { get; set; }
        public int UniqueCustomers { get; set; }

        public decimal TotalDiscounts { get; set; }
        public decimal TotalShipping { get; set; }
        public decimal TotalTax { get; set; }
    }

    public class SalesTimelineDto
    {
        public DateTime Date { get; set; }
        public string Label { get; set; } = null!; // "Jan 01", "Week 1", etc.
        public decimal Revenue { get; set; }
        public int Orders { get; set; }
        public decimal AverageOrderValue { get; set; }
    }

    public class SalesByCategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public decimal Revenue { get; set; }
        public int Orders { get; set; }
        public int ItemsSold { get; set; }
        public decimal Percentage { get; set; } // % of total sales
    }

    public class SalesByBrandDto
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; } = null!;
        public decimal Revenue { get; set; }
        public int Orders { get; set; }
        public int ItemsSold { get; set; }
        public decimal Percentage { get; set; }
    }

    public class SalesByPaymentMethodDto
    {
        public string PaymentMethod { get; set; } = null!;
        public decimal Revenue { get; set; }
        public int Orders { get; set; }
        public decimal Percentage { get; set; }
    }

    public class OrderStatusDistributionDto
    {
        public string Status { get; set; } = null!;
        public int Count { get; set; }
        public decimal Revenue { get; set; }
        public decimal Percentage { get; set; }
    }

    public class SalesComparisonDto
    {
        public decimal RevenueChange { get; set; } // +15.5%
        public decimal OrdersChange { get; set; }
        public decimal AverageOrderValueChange { get; set; }
        public decimal CustomersChange { get; set; }

        public SalesSummaryDto CurrentPeriod { get; set; } = null!;
        public SalesSummaryDto PreviousPeriod { get; set; } = null!;
    }

    // ========================================
    // LEGACY SUPPORT (for backward compatibility)
    // ========================================

    public class SalesAnalyticsFilterDto
    {
        public string Period { get; set; } = "30days"; // 7days, 30days, 90days, year
        public string GroupBy { get; set; } = "day"; // day, week, month
    }
}
