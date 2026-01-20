using System;
using System.Collections.Generic;
using System.Text;
using YMM.Data.Entities.Identity;

namespace YMM.Data.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string PostalCode { get; set; } = null!;

        public bool IsDefault { get; set; }

        public User User { get; set; } = null!;
    }

}
