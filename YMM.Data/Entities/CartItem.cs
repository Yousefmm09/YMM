using System;
using System.Collections.Generic;
using System.Text;

namespace YMM.Data.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductVariantId { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

}
