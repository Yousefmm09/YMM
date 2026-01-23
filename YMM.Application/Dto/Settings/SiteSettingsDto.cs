using System.ComponentModel.DataAnnotations;

namespace YMM.Application.Dto.Settings
{
    public class SiteSettingsDto
    {
        public string SiteName { get; set; } = null!;
        public string Currency { get; set; } = null!;
        public decimal TaxRate { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal FreeShippingThreshold { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
        public string? Address { get; set; }
        public string? FacebookUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string? TwitterUrl { get; set; }
    }

    public class UpdateSiteSettingsDto
    {
        [MaxLength(100)]
        public string? SiteName { get; set; }

        [MaxLength(10)]
        public string? Currency { get; set; }

        [Range(0, 1)]
        public decimal? TaxRate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? ShippingFee { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? FreeShippingThreshold { get; set; }

        [EmailAddress]
        [MaxLength(255)]
        public string? ContactEmail { get; set; }

        [Phone]
        [MaxLength(20)]
        public string? ContactPhone { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }

        [Url]
        [MaxLength(500)]
        public string? FacebookUrl { get; set; }

        [Url]
        [MaxLength(500)]
        public string? InstagramUrl { get; set; }

        [Url]
        [MaxLength(500)]
        public string? TwitterUrl { get; set; }
    }
}
