using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YMM.Application.Dto.Order
{
    public class ReturnRequestDto
    {
        [Required]
        [MaxLength(500)]
        public string Reason { get; set; } = null!;

        [Required]
        [MinLength(1, ErrorMessage = "At least one item is required")]
        public List<ReturnItemDto> Items { get; set; } = new List<ReturnItemDto>();
    }

    public class ReturnItemDto
    {
        [Required]
        public int OrderItemId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }
}
