using System.Collections.Generic;
using System.Linq;
using Ecart.Products;

namespace Ecart.Cart
{
    public class ShoppingCart
    {
        private readonly List<CartItem> _items = new List<CartItem>();

        public IReadOnlyList<CartItem> Items => _items.AsReadOnly();

        public void Add(Product p, int qty = 1)
        {
            var item = _items.FirstOrDefault(i => i.Product.Id == p.Id);
            if (item == null)
            {
                _items.Add(new CartItem { Product = p, Quantity = qty });
            }
            else
            {
                item.Quantity += qty;
            }
        }

        public void Remove(int productId)
        {
            var item = _items.FirstOrDefault(i => i.Product.Id == productId);
            if (item != null) _items.Remove(item);
        }

        public decimal GetTotal() => _items.Sum(i => i.Product.Price * i.Quantity);

        public void Clear() => _items.Clear();
    }
}
