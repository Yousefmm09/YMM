using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YMM.Data.Entities
{
    public class ProductVariant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [MaxLength(50)]
        public string? Size { get; set; } // 40, 41, 42, etc.

        [MaxLength(50)]
        public string? Color { get; set; } // Black, White, Red, etc.

        [MaxLength(100)]
        public string? SKU { get; set; } // Unique SKU for variant

        public int StockQuantity { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal? PriceAdjustment { get; set; } // Extra cost for this variant

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } = null!;

        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
