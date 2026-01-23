using System.ComponentModel.DataAnnotations;

namespace YMM.Application.Dto.Cart
{
    public class AddToCartDto
    {
        [Required]
        public int ProductId { get; set; }

        public int? VariantId { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
        public int Quantity { get; set; } = 1;
    }
}
