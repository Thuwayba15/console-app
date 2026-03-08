using OnlineShoppingSystem.Models;
using OnlineShoppingSystem.Enums;

namespace OnlineShoppingSystem.Data;

/// <summary>
/// Provides seed data for the application
/// </summary>
public static class SeedData
{
    /// <summary>
    /// Initialize the data store with sample users and products
    /// </summary>
    public static void Initialize()
    {
        var dataStore = AppDataStore.Instance;

        // Create admin user
        var admin = new Administrator
        {
            Id = dataStore.GetNextUserId(),
            Username = "admin",
            Email = "admin@shop.com",
            Password = "admin123",
            CreatedAt = DateTime.Now
        };
        dataStore.Users.Add(admin);

        // Create customer user
        var customer = new Customer
        {
            Id = dataStore.GetNextUserId(),
            Username = "customer1",
            Email = "customer1@email.com",
            Password = "customer123",
            WalletBalance = 500.00m,
            CreatedAt = DateTime.Now
        };
        dataStore.Users.Add(customer);

        // Create sample products
        var products = new List<Product>
        {
            new Product
            {
                Id = dataStore.GetNextProductId(),
                Name = "Laptop",
                Description = "High-performance laptop for work and gaming",
                Price = 999.99m,
                StockQuantity = 15,
                Category = "Electronics"
            },
            new Product
            {
                Id = dataStore.GetNextProductId(),
                Name = "Wireless Mouse",
                Description = "Ergonomic wireless mouse with long battery life",
                Price = 29.99m,
                StockQuantity = 50,
                Category = "Electronics"
            },
            new Product
            {
                Id = dataStore.GetNextProductId(),
                Name = "Mechanical Keyboard",
                Description = "RGB mechanical keyboard with blue switches",
                Price = 79.99m,
                StockQuantity = 30,
                Category = "Electronics"
            },
            new Product
            {
                Id = dataStore.GetNextProductId(),
                Name = "USB-C Cable",
                Description = "Durable USB-C charging cable 6ft",
                Price = 12.99m,
                StockQuantity = 100,
                Category = "Accessories"
            },
            new Product
            {
                Id = dataStore.GetNextProductId(),
                Name = "Monitor 27 inch",
                Description = "4K UHD monitor with HDR support",
                Price = 349.99m,
                StockQuantity = 20,
                Category = "Electronics"
            },
            new Product
            {
                Id = dataStore.GetNextProductId(),
                Name = "Desk Lamp",
                Description = "LED desk lamp with adjustable brightness",
                Price = 39.99m,
                StockQuantity = 8,
                Category = "Office Supplies"
            },
            new Product
            {
                Id = dataStore.GetNextProductId(),
                Name = "Notebook Set",
                Description = "Pack of 5 premium quality notebooks",
                Price = 19.99m,
                StockQuantity = 45,
                Category = "Office Supplies"
            },
            new Product
            {
                Id = dataStore.GetNextProductId(),
                Name = "Wireless Headphones",
                Description = "Noise-cancelling Bluetooth headphones",
                Price = 149.99m,
                StockQuantity = 25,
                Category = "Electronics"
            },
            new Product
            {
                Id = dataStore.GetNextProductId(),
                Name = "Phone Stand",
                Description = "Adjustable aluminum phone stand",
                Price = 24.99m,
                StockQuantity = 3,
                Category = "Accessories"
            },
            new Product
            {
                Id = dataStore.GetNextProductId(),
                Name = "Webcam HD",
                Description = "1080p webcam with built-in microphone",
                Price = 69.99m,
                StockQuantity = 18,
                Category = "Electronics"
            }
        };

        dataStore.Products.AddRange(products);

        // Create a cart for the customer
        var cart = new Cart
        {
            Id = dataStore.GetNextCartId(),
            CustomerId = customer.Id
        };
        dataStore.Carts.Add(cart);
    }
}
