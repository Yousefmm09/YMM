using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace YMM.Data.Entities.Identity
{
    public class User:IdentityUser
    {
            public int Age { get; set; }
            public string Country { get; set; }
            public bool IsActive { get; set; } = true;
        public ICollection<Address> Adress { get; set; }
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

            public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

            public DateTime? LastLogin { get; set; }

            public UserProfile? Profile { get; set; }
        public ICollection<Review> Reviews { get; set; }
       = new List<Review>();

    }
}
