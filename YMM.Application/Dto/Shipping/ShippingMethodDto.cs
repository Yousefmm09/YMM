namespace YMM.Application.Dto.Shipping
{
    public class ShippingMethodDto
    {
        public string Code { get; set; } = null!; // Standard, Express, SameDay
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string EstimatedDelivery { get; set; } = null!; // e.g., "3-5 business days"
        public bool IsAvailable { get; set; }
    }
}
