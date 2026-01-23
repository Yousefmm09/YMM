namespace YMM.Application.Dto.Admin
{
    public class DashboardStatsDto
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalProducts { get; set; }
        public int PendingOrders { get; set; }
        public int LowStockProducts { get; set; }
        public int NewCustomersToday { get; set; }
        public int OrdersToday { get; set; }
        public decimal RevenueToday { get; set; }
        public decimal AverageOrderValue { get; set; }
    }
}
