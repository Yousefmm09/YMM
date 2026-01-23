using System;
using System.Collections.Generic;

namespace YMM.Application.Dto.Cart
{
    public class CartDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public string? CouponCode { get; set; }
        public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CartItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int? VariantId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? ProductImage { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public int AvailableStock { get; set; }
    }
}
