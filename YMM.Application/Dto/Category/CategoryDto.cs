using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Data.Entities;

namespace YMM.Application.Dto.Category
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Slug { get; set; }
        public bool IsActive { get; set; }
        public int ProductCount {  get; set; }
        //public ICollection<YMM.Product> Products { get; set; }= new List<YMM.Data.Entities.Product>();


    }
}
