using System;
using System.Collections.Generic;
using System.Text;

namespace YMM.Data.Entities.Identity
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public string Token { get; set; } = null!;

        public DateTime ExpiresAt { get; set; }

        public bool IsRevoked { get; set; }

        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
