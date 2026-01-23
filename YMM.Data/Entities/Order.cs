using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YMM.Data.Entities.Identity;

namespace YMM.Data.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string OrderNumber { get; set; } = null!; // ORD-2026-0001

        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        public int ShippingAddressId { get; set; }

        [Required]
        public int BillingAddressId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Pending";
        // Pending, Confirmed, Processing, Shipped, Delivered, Cancelled, Returned, Refunded

        [Required]
        [MaxLength(50)]
        public string PaymentStatus { get; set; } = "Pending";
        // Pending, Paid, Failed, Refunded

        [Required]
        [MaxLength(50)]
        public string PaymentMethod { get; set; } = null!;
        // CreditCard, DebitCard, PayPal, CashOnDelivery

        [MaxLength(100)]
        public string? TransactionId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Discount { get; set; } = 0;

        public int? CouponId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Tax { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingCost { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        public string? CancellationReason { get; set; }

        public DateTime? CancelledAt { get; set; }

        public DateTime? DeliveredAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("ShippingAddressId")]
        public virtual Address ShippingAddress { get; set; } = null!;

        [ForeignKey("BillingAddressId")]
        public virtual Address BillingAddress { get; set; } = null!;

        [ForeignKey("CouponId")]
        public virtual Coupon? Coupon { get; set; }

        public virtual ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public virtual ICollection<OrderStatusHistory> StatusHistory { get; set; } = new List<OrderStatusHistory>();
        public virtual Shipment? Shipment { get; set; }
        public virtual Payment? Payment { get; set; }
    }
}
