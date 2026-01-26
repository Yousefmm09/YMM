using System;
using System.Collections.Generic;

namespace YMM.Application.Dto.Shipping
{
    public class ShipmentDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = null!;
        public string TrackingNumber { get; set; } = null!;
        public string Carrier { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string? CurrentLocation { get; set; }
        public DateTime? ShippedAt { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<ShipmentTrackingDto> TrackingHistory { get; set; } = new List<ShipmentTrackingDto>();
    }
}
