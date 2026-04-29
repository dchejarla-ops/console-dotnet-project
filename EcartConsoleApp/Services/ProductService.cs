using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using Ecart.Products;

namespace Ecart.Services
{
    public static class ProductService
    {
        public static List<Product> GetAll()
        {
            try
            {
                var path = Path.Combine(AppContext.BaseDirectory, "Products", "products.json");
                if (!File.Exists(path)) return new List<Product>();

                var json = File.ReadAllText(path);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var products = JsonSerializer.Deserialize<List<Product>>(json, options);
                return products ?? new List<Product>();
            }
            catch
            {
                return new List<Product>();
            }
        }

        public static Product? GetById(int id) => GetAll().Find(p => p.Id == id);
    }
}
