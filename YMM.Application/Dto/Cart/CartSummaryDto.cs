namespace YMM.Application.Dto.Cart
{
    public class CartSummaryDto
    {
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal Shipping { get; set; }
        public decimal Total { get; set; }
        public int ItemCount { get; set; }
        public string? CouponCode { get; set; }
        public string? CouponDescription { get; set; }
    }
}
