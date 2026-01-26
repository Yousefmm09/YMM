using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Dto.Product;

namespace YMM.Application.Dto.Category
{
    public class CategoryDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public bool IsActive { get; set; }
        public int ProductCount { get; set; }
        public List<ProductDto> Products { get; set; }
    }

}
