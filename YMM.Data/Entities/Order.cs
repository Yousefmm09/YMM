using System;
using System.Collections.Generic;
using System.Text;
using YMM.Data.Entities.Identity;

namespace YMM.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public virtual User User { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
    public enum OrderStatus
    {
        Pending,
        Paid,
        Shipped,
        Delivered,
        Cancelled
    }

}
