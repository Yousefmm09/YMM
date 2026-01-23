using System;

namespace YMM.Application.Dto.Shipping
{
    public class ShipmentTrackingDto
    {
        public int Id { get; set; }
        public string Status { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
