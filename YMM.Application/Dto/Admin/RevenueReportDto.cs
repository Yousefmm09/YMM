using System;
using System.Collections.Generic;

namespace YMM.Application.Dto.Admin
{
    public class RevenueReportDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalShipping { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal NetRevenue { get; set; }
        public decimal TotalRefunds { get; set; }
        public int TotalOrders { get; set; }
        public int CompletedOrders { get; set; }
        public int CancelledOrders { get; set; }
        public int RefundedOrders { get; set; }
        public List<RevenueByPaymentMethodDto> RevenueByPaymentMethod { get; set; } = new List<RevenueByPaymentMethodDto>();
    }

    public class RevenueByPaymentMethodDto
    {
        public string PaymentMethod { get; set; } = null!;
        public decimal Revenue { get; set; }
        public int OrderCount { get; set; }
        public decimal Percentage { get; set; }
    }

    public class RevenueReportFilterDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
