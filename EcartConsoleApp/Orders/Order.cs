using System;
using System.Collections.Generic;
using Ecart.Cart;

namespace Ecart.Orders
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal Total { get; set; }
    }
}
