using System;
using System.Collections.Generic;
using System.Text;

namespace YMM.Data.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int  ProductVariantId { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

}
