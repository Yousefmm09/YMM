using System.ComponentModel.DataAnnotations;

namespace YMM.Application.Dto.ProductDtos
{
    public class UpdateStockDto
    {
        [Required]
        public string Size { get; set; } = null!;

        [Required]
        public string Color { get; set; } = null!;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required]
        public string Operation { get; set; } = null!; // add, subtract, set
    }
}
