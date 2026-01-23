using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YMM.Data.Entities
{
    public class ShipmentTracking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ShipmentId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public string Location { get; set; } = null!;

        [MaxLength(500)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("ShipmentId")]
        public virtual Shipment Shipment { get; set; } = null!;
    }
}
