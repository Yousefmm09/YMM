using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YMM.Application.Dto.ProductDtos
{
    public class BulkUpdateProductDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "At least one product ID is required")]
        public List<int> ProductIds { get; set; } = new List<int>();

        public decimal? SalePrice { get; set; }
        public bool? IsFeatured { get; set; }
        public bool? IsNew { get; set; }
        public bool? IsActive { get; set; }
        public int? CategoryId { get; set; }
    }
}
