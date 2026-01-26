using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YMM.Data.Entities
{
    public class Shipment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        [MaxLength(100)]
        public string TrackingNumber { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Carrier { get; set; } = null!; // DHL, FedEx, UPS, etc.

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Preparing";
        // Preparing, InTransit, OutForDelivery, Delivered, Failed

        [MaxLength(255)]
        public string? CurrentLocation { get; set; }

        public DateTime? ShippedAt { get; set; }

        public DateTime? EstimatedDeliveryDate { get; set; }

        public DateTime? DeliveredAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; } = null!;

        public virtual ICollection<ShipmentTracking> TrackingHistory { get; set; } = new List<ShipmentTracking>();
    }
}
