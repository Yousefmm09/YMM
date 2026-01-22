using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMM.Data.Entities.Identity
{
    public class OtpEmail
    {
        public int Id { get; set; }
        public string UserId { get;  set; }
        public virtual User User { get; set; }
        public string OtpHash { get;  set; }
        public DateTime ExpiresAt { get;  set; }
        public int Attmeps {  get; set; }
        public bool IsUsed { get;  set; }
        public bool IsExpired() => DateTime.UtcNow > ExpiresAt;
        public DateTime CreatedAt { get;  set; }

    }
}
