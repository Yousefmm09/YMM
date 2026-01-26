using System.ComponentModel.DataAnnotations;

namespace YMM.Application.Dto.Wishlist
{
    public class MoveToCartDto
    {
        public int? VariantId { get; set; }

        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
        public int Quantity { get; set; } = 1;
    }
}
