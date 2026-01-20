using System;
using System.Collections.Generic;
using System.Text;
using YMM.Data.Entities.Identity;

namespace YMM.Data.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }

}
