using System.ComponentModel.DataAnnotations;

namespace YMM.Application.Dto.Wishlist
{
    public class AddToWishlistDto
    {
        [Required]
        public int ProductId { get; set; }
    }
}
