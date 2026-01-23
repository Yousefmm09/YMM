using System;
using System.Collections.Generic;

namespace YMM.Application.Dto.Order
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string PaymentStatus { get; set; } = null!;
        public string PaymentMethod { get; set; } = null!;
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal Total { get; set; }
        public string? Notes { get; set; }
        public string? CancellationReason { get; set; }
        public DateTime? CancelledAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        public OrderAddressDto ShippingAddress { get; set; } = null!;
        public OrderAddressDto BillingAddress { get; set; } = null!;
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
        public OrderShipmentDto? Shipment { get; set; }
        public OrderPaymentDto? Payment { get; set; }
        public List<OrderStatusHistoryDto> StatusHistory { get; set; } = new List<OrderStatusHistoryDto>();
    }

    public class OrderAddressDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string? Building { get; set; }
        public string? Apartment { get; set; }
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string Country { get; set; } = null!;
    }

    public class OrderShipmentDto
    {
        public int Id { get; set; }
        public string TrackingNumber { get; set; } = null!;
        public string Carrier { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string? CurrentLocation { get; set; }
        public DateTime? ShippedAt { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public DateTime? DeliveredAt { get; set; }
    }

    public class OrderPaymentDto
    {
        public int Id { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Status { get; set; } = null!;
        public string? TransactionId { get; set; }
        public DateTime? PaidAt { get; set; }
    }

    public class OrderStatusHistoryDto
    {
        public int Id { get; set; }
        public string Status { get; set; } = null!;
        public string? Notes { get; set; }
        public string? ChangedByName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
