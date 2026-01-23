using System.ComponentModel.DataAnnotations;

namespace YMM.Application.Dto.Order
{
    public class UpdateOrderStatusDto
    {
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = null!; // Pending, Confirmed, Processing, Shipped, Delivered, Cancelled, Returned, Refunded

        [MaxLength(100)]
        public string? TrackingNumber { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }
    }
}
