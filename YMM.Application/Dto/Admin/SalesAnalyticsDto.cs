using System;
using System.Collections.Generic;

namespace YMM.Application.Dto.Admin
{
    public class SalesAnalyticsDto
    {
        public string Period { get; set; } = null!; // 7days, 30days, 90days, year
        public string GroupBy { get; set; } = null!; // day, week, month
        public decimal TotalSales { get; set; }
        public int TotalOrders { get; set; }
        public decimal AverageOrderValue { get; set; }
        public decimal GrowthPercentage { get; set; }
        public List<SalesDataPointDto> DataPoints { get; set; } = new List<SalesDataPointDto>();
    }

    public class SalesDataPointDto
    {
        public DateTime Date { get; set; }
        public string Label { get; set; } = null!;
        public decimal Sales { get; set; }
        public int Orders { get; set; }
    }

    public class SalesAnalyticsFilterDto
    {
        public string Period { get; set; } = "30days"; // 7days, 30days, 90days, year
        public string GroupBy { get; set; } = "day"; // day, week, month
    }
}
