using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMM.Application.Dto.Product
{
    public class AddProductDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public List<ProductVariantDto> Variants { get; set; } = new List<ProductVariantDto>();
    }
}
