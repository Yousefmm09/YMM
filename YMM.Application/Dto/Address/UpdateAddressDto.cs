using System.ComponentModel.DataAnnotations;

namespace YMM.Application.Dto.Address
{
    public class UpdateAddressDto
    {
        [MaxLength(100)]
        public string? FullName { get; set; }

        [Phone]
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(255)]
        public string? Street { get; set; }

        [MaxLength(255)]
        public string? Building { get; set; }

        [MaxLength(100)]
        public string? Apartment { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(100)]
        public string? State { get; set; }

        [MaxLength(20)]
        public string? PostalCode { get; set; }

        [MaxLength(100)]
        public string? Country { get; set; }

        [MaxLength(20)]
        public string? AddressType { get; set; }
    }
}
