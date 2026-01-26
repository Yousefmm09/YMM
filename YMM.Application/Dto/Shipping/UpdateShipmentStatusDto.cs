using System.ComponentModel.DataAnnotations;

namespace YMM.Application.Dto.Shipping
{
    public class UpdateShipmentStatusDto
    {
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = null!; // Preparing, InTransit, OutForDelivery, Delivered, Failed

        [Required]
        [MaxLength(255)]
        public string Location { get; set; } = null!;

        [MaxLength(500)]
        public string? Notes { get; set; }
    }
}
