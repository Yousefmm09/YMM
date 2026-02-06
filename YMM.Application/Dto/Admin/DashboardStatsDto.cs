using System;
using System.Collections.Generic;

namespace YMM.Application.Dto.Admin
{
    public class DashboardStatsDto
    {
        // ========================================
        // OVERVIEW STATS
        // ========================================
        public decimal TotalRevenue { get; set; }
        public decimal TotalRevenueChange { get; set; } // +15.5% for example

        public int TotalOrders { get; set; }
        public decimal TotalOrdersChange { get; set; } // +8.2%

        public int TotalCustomers { get; set; }
        public decimal TotalCustomersChange { get; set; } // +12.3%

        public int TotalProducts { get; set; }
        public int ActiveProducts { get; set; }

        public decimal AverageOrderValue { get; set; }
        public decimal AverageOrderValueChange { get; set; }

        // ========================================
        // TODAY'S STATS
        // ========================================
        public TodayStatsDto Today { get; set; } = null!;

        // ========================================
        // THIS WEEK
        // ========================================
        public WeekStatsDto ThisWeek { get; set; } = null!;

        // ========================================
        // THIS MONTH
        // ========================================
        public MonthStatsDto ThisMonth { get; set; } = null!;

        // ========================================
        // ALERTS & NOTIFICATIONS
        // ========================================
        public DashboardAlertsDto Alerts { get; set; } = null!;

        // ========================================
        // QUICK STATS
        // ========================================
        public int PendingOrders { get; set; }
        public int ProcessingOrders { get; set; }
        public int ShippedOrders { get; set; }
        public int LowStockProducts { get; set; }
        public int OutOfStockProducts { get; set; }
        public int PendingReviews { get; set; }
        public int NewCustomersThisMonth { get; set; }

        // ========================================
        // RECENT ACTIVITY
        // ========================================
        public List<RecentOrderDto> RecentOrders { get; set; } = new List<RecentOrderDto>();
        public List<RecentCustomerDto> RecentCustomers { get; set; } = new List<RecentCustomerDto>();

        // ========================================
        // CHARTS DATA
        // ========================================
        public List<ChartDataPointDto> SalesChartData { get; set; } = new List<ChartDataPointDto>(); // Last 7 days
        public List<ChartDataPointDto> OrdersChartData { get; set; } = new List<ChartDataPointDto>(); // Last 7 days
    }

    // ========================================
    // SUPPORTING DTOs
    // ========================================

    public class TodayStatsDto
    {
        public decimal Revenue { get; set; }
        public int Orders { get; set; }
        public int NewCustomers { get; set; }
        public int Visitors { get; set; }
    }

    public class WeekStatsDto
    {
        public decimal Revenue { get; set; }
        public int Orders { get; set; }
        public int NewCustomers { get; set; }
        public decimal AverageOrderValue { get; set; }
    }

    public class MonthStatsDto
    {
        public decimal Revenue { get; set; }
        public int Orders { get; set; }
        public int NewCustomers { get; set; }
        public decimal GrowthRate { get; set; } // Compared to last month
    }

    public class DashboardAlertsDto
    {
        public int LowStockCount { get; set; }
        public int OutOfStockCount { get; set; }
        public int PendingOrdersCount { get; set; }
        public int PendingReviewsCount { get; set; }
        public int FailedPaymentsCount { get; set; }
        public List<string> CriticalAlerts { get; set; } = new List<string>();
    }

    public class RecentOrderDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = null!;
        public string CustomerName { get; set; } = null!;
        public decimal Total { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }

    public class RecentCustomerDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime RegisteredAt { get; set; }
        public int OrdersCount { get; set; }
    }

    public class ChartDataPointDto
    {
        public string Label { get; set; } = null!; // Date or Category
        public decimal Value { get; set; }
        public string? Color { get; set; } // Optional for charts
    }
}
