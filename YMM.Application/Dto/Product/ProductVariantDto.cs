using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMM.Application.Dto.Product
{
    public class ProductVariantDto
    {
        public string Size { get; set; } = null!;
        public string Color { get; set; } = null!;
        public int StockQuantity { get; set; }
    }
}
