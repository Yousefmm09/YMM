using System;
using System.Collections.Generic;
using System.Text;
using YMM.Data.Entities.Identity;

namespace YMM.Data.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }

        public int Rating { get; set; } // 1–5
        public string Comment { get; set; } = null!;
        public Product Product { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
