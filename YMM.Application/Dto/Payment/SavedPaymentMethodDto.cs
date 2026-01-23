using System;

namespace YMM.Application.Dto.Payment
{
    public class SavedPaymentMethodDto
    {
        public int Id { get; set; }
        public string Type { get; set; } = null!;
        public string CardHolderName { get; set; } = null!;
        public string Last4Digits { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public bool IsDefault { get; set; }
        public bool IsExpired => new DateTime(ExpiryYear, ExpiryMonth, 1).AddMonths(1) < DateTime.UtcNow;
        public DateTime CreatedAt { get; set; }
    }
}
