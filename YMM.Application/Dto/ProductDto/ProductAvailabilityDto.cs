namespace YMM.Application.Dto.Product
{
    public class ProductAvailabilityDto
    {
        public int ProductId { get; set; }
        public int? VariantId { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public bool IsAvailable { get; set; }
        public int StockQuantity { get; set; }
    }

    public class CheckAvailabilityDto
    {
        public string? Size { get; set; }
        public string? Color { get; set; }
    }
}
