using System.ComponentModel.DataAnnotations;

namespace YMM.Application.Dto.Address
{
    public class CreateAddressDto
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;

        [Required]
        [Phone]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public string Street { get; set; } = null!;

        [MaxLength(255)]
        public string? Building { get; set; }

        [MaxLength(100)]
        public string? Apartment { get; set; }

        [Required]
        [MaxLength(100)]
        public string City { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string State { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string PostalCode { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Country { get; set; } = "Egypt";

        public bool IsDefault { get; set; } = false;

        [MaxLength(20)]
        public string AddressType { get; set; } = "Home"; // Home, Work, Other
    }
}
