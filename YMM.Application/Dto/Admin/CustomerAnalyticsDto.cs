using System;
using System.Collections.Generic;

namespace YMM.Application.Dto.Admin
{
    public class CustomerAnalyticsDto
    {
        public string Period { get; set; } = null!;
        public int TotalCustomers { get; set; }
        public int NewCustomers { get; set; }
        public int ActiveCustomers { get; set; }
        public int ReturningCustomers { get; set; }
        public decimal CustomerRetentionRate { get; set; }
        public decimal AverageOrdersPerCustomer { get; set; }
        public decimal AverageCustomerLifetimeValue { get; set; }
        public List<CustomerGrowthDataPointDto> GrowthData { get; set; } = new List<CustomerGrowthDataPointDto>();
        public List<TopCustomerDto> TopCustomers { get; set; } = new List<TopCustomerDto>();
    }

    public class CustomerGrowthDataPointDto
    {
        public DateTime Date { get; set; }
        public string Label { get; set; } = null!;
        public int NewCustomers { get; set; }
        public int TotalCustomers { get; set; }
    }

    public class TopCustomerDto
    {
        public string UserId { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? FullName { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
        public DateTime LastOrderDate { get; set; }
    }

    public class CustomerAnalyticsFilterDto
    {
        public string Period { get; set; } = "30days";
    }
}
