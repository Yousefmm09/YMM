using System;
using YMM.Application.Dto.Common;

namespace YMM.Application.Dto.Order
{
    public class OrderFilterDto : PaginationParams
    {
        public string? Status { get; set; }
        public string? PaymentStatus { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Search { get; set; } // Order number or customer name
    }
}
