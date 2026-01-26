using System.ComponentModel.DataAnnotations;

namespace YMM.Application.Dto.Order
{
    public class CancelOrderDto
    {
        [Required]
        [MaxLength(500)]
        public string Reason { get; set; } = null!;
    }
}
