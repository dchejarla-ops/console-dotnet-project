using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using Ecart.Products;
using Ecart.Services;
using Ecart.Cart;
using Ecart.Orders;
using Ecart.Payments;

namespace Ecart;

class Program
{
    static ShoppingCart cart = new ShoppingCart();

    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Main menu");
            Console.WriteLine();
            Console.WriteLine("1. View Product List");
            Console.WriteLine("2. Add Product");
            Console.WriteLine("3. Remove Product");
            Console.WriteLine("4. View Cart");
            Console.WriteLine("5. Payment");
            Console.WriteLine("6. Clear Cart");
            Console.WriteLine("7. Exit");
            Console.WriteLine();
            Console.Write("Select an option: ");
            var input = Console.ReadLine();
            if (!int.TryParse(input, out var sel)) continue;

            switch (sel)
            {
                case 1: ViewProducts(); break;
                case 2: AddProduct(); break;
                case 3: RemoveProduct(); break;
                case 4: ViewCart(); break;
                case 5: Payment(); break;
                case 6: ClearCart(); break;
                case 7: return;
            }
        }
    }

    static void ViewProducts()
    {
        var products = ProductService.GetAll();
        Console.Clear();
        if (products.Count == 0)
        {
            Console.WriteLine("No products found.");
            var path = Path.Combine(AppContext.BaseDirectory, "Products", "products.json");
            Console.WriteLine($"Expected JSON path: {path}");
            Console.WriteLine($"File exists: {File.Exists(path)}");
            Console.WriteLine();
            Console.Write("Press Enter to return to Main menu...");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("Products:");
        foreach (var p in products)
        {
            Console.WriteLine($"{p.Id}. {p.Name} - {p.Price:C}");
        }
        Console.WriteLine();
        Console.Write("Press Enter to return to Main menu...");
        Console.ReadLine();
        return;
    }

    static void AddProduct()
    {
        var products = ProductService.GetAll();
        Console.Clear();
        if (products.Count == 0)
        {
            Console.WriteLine("No products available to add.");
            return;
        }

        Console.WriteLine("Products:");
        foreach (var p in products)
        {
            Console.WriteLine($"{p.Id}. {p.Name} - {p.Price:C}");
        }

        Console.Write("Enter product id to add to cart: ");
        var line = Console.ReadLine();
        if (!int.TryParse(line, out var id)) return;
        var prod = ProductService.GetById(id);
        if (prod == null) return;

        Console.Write("Enter quantity: ");
        var qline = Console.ReadLine();
        var qty = 1;
        if (!int.TryParse(qline, out qty) || qty < 1) qty = 1;

        cart.Add(prod, qty);
        Console.WriteLine();
        Console.WriteLine("Product added to cart.");
        Console.Write("Press Enter to return to Main menu...");
        Console.ReadLine();
    }

    static void RemoveProduct()
    {
        Console.Clear();
        if (cart.Items.Count == 0)
        {
            Console.WriteLine("Cart is empty.");
            return;
        }

        Console.WriteLine("Cart items:");
        foreach (var item in cart.Items)
        {
            Console.WriteLine($"{item.Product.Id}. {item.Product.Name} x{item.Quantity}");
        }

        Console.Write("Enter product id to remove from cart: ");
        var line = Console.ReadLine();
        if (!int.TryParse(line, out var id)) return;
        cart.Remove(id);
        Console.WriteLine();
        Console.WriteLine("Product removed from cart if it existed.");
        Console.Write("Press Enter to return to Main menu...");
        Console.ReadLine();
    }

    static void ViewCart()
    {
        Console.Clear();
        Console.WriteLine("Cart:");
        if (cart.Items.Count == 0)
        {
            Console.WriteLine("Cart is empty.");
            Console.WriteLine();
            Console.Write("Press Enter to return to Main menu...");
            Console.ReadLine();
            return;
        }

        for (int i = 0; i < cart.Items.Count; i++)
        {
            var item = cart.Items[i];
            Console.WriteLine($"{i+1}. {item.Product.Name} x{item.Quantity} - {item.Product.Price:C} each");
        }

        Console.WriteLine();
        Console.Write("Press Enter to return to Main menu...");
        Console.ReadLine();
        return;
    }

    static void PlaceOrder()
    {
        if (cart.Items.Count == 0)
        {
            Console.WriteLine("Cart is empty.");
            return;
        }

        Console.Clear();
        Console.WriteLine("Order Summary:");
        foreach (var item in cart.Items)
        {
            Console.WriteLine($"- {item.Product.Name} x{item.Quantity} = {item.Product.Price * item.Quantity:C}");
        }

        Console.WriteLine($"Total: {cart.GetTotal():C}");
        Console.WriteLine();
        Console.WriteLine("Select payment method:");
        Console.WriteLine("1. Credit Card");
        Console.WriteLine("2. PayPal");
        Console.Write("Choice: ");
        var choice = Console.ReadLine();

        IPaymentProcessor processor = choice == "2" ? new PaypalProcessor() as IPaymentProcessor : new CreditCardProcessor();
        var success = processor.ProcessPayment(cart.GetTotal());
        if (success)
        {
            var order = new Order { Items = new System.Collections.Generic.List<CartItem>(cart.Items), Total = cart.GetTotal() };
            cart.Clear();
            Console.WriteLine($"Order placed. Total: {order.Total:C}");
        }
        Console.WriteLine();
        Console.Write("Press Enter to return to Main menu...");
        Console.ReadLine();
        return;
    }

    static void Payment()
    {
        // reuse PlaceOrder for payment flow
        PlaceOrder();
    }

    static void ClearCart()
    {
        cart.Clear();
        Console.WriteLine();
        Console.WriteLine("Cart cleared.");
        Console.Write("Press Enter to return to Main menu...");
        Console.ReadLine();
    }
}
