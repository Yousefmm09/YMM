using System;
using System.Collections.Generic;
using System.Text;

namespace YMM.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }

        public int BrandId { get; set; }
        public int CategoryId { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public Brand Brand { get; set; } = null!;
        public Category Category { get; set; } = null!;

        public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public ICollection<Review> Reviews { get; set; }
       = new List<Review>();
    }

}
