using YMM.Application.Dto.Common;

namespace YMM.Application.Dto.Shipping
{
    public class ShipmentFilterDto : PaginationParams
    {
        public string? Status { get; set; }
        public string? Carrier { get; set; }
        public string? Search { get; set; } // Tracking number or order number
    }
}
