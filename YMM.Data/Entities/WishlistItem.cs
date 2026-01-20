using System;
using System.Collections.Generic;
using System.Text;

namespace YMM.Data.Entities
{
    public class WishlistItem
    {
        public int Id { get; set; }
        public int WishlistId { get; set; }
        public int ProductId { get; set; }
    }

}
