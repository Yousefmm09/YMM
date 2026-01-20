using System;
using System.Collections.Generic;
using System.Text;
using YMM.Data.Entities.Identity;

namespace YMM.Data.Entities
{
    public class Wishlist
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public ICollection<WishlistItem> Items { get; set; } = new List<WishlistItem>();
    }

}
