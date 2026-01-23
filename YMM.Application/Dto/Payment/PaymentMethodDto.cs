namespace YMM.Application.Dto.Payment
{
    public class PaymentMethodDto
    {
        public string Code { get; set; } = null!; // CreditCard, DebitCard, PayPal, CashOnDelivery
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? IconUrl { get; set; }
        public bool IsAvailable { get; set; }
    }
}
