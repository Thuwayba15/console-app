using OnlineShoppingSystem.Data;
using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Services;

/// <summary>
/// Service for managing products
/// </summary>
public class ProductService : IProductService
{
    private readonly AppDataStore _dataStore;

    public ProductService()
    {
        _dataStore = AppDataStore.Instance;
    }

    public List<Product> GetAllProducts()
    {
        return _dataStore.Products.ToList();
    }

    public Product? GetProductById(int productId)
    {
        return _dataStore.Products.FirstOrDefault(p => p.Id == productId);
    }

    public List<Product> SearchProducts(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return GetAllProducts();
        }

        return _dataStore.Products
            .Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                       p.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                       p.Category.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public Product AddProduct(string name, string description, decimal price, int stockQuantity, string category)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Product name is required.");
        }

        if (price <= 0)
        {
            throw new ArgumentException("Price must be greater than zero.");
        }

        if (stockQuantity < 0)
        {
            throw new ArgumentException("Stock quantity cannot be negative.");
        }

        var product = new Product
        {
            Id = _dataStore.GetNextProductId(),
            Name = name,
            Description = description,
            Price = price,
            StockQuantity = stockQuantity,
            Category = category
        };

        _dataStore.Products.Add(product);
        return product;
    }

    public bool UpdateProduct(int productId, string name, string description, decimal price, int stockQuantity, string category)
    {
        var product = GetProductById(productId);
        if (product == null)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Product name is required.");
        }

        if (price <= 0)
        {
            throw new ArgumentException("Price must be greater than zero.");
        }

        if (stockQuantity < 0)
        {
            throw new ArgumentException("Stock quantity cannot be negative.");
        }

        product.Name = name;
        product.Description = description;
        product.Price = price;
        product.StockQuantity = stockQuantity;
        product.Category = category;

        return true;
    }

    public bool DeleteProduct(int productId)
    {
        var product = GetProductById(productId);
        if (product == null)
        {
            return false;
        }

        _dataStore.Products.Remove(product);
        return true;
    }

    public bool RestockProduct(int productId, int additionalQuantity)
    {
        var product = GetProductById(productId);
        if (product == null)
        {
            return false;
        }

        if (additionalQuantity <= 0)
        {
            throw new ArgumentException("Additional quantity must be greater than zero.");
        }

        product.StockQuantity += additionalQuantity;
        return true;
    }

    public List<Product> GetLowStockProducts(int threshold)
    {
        return _dataStore.Products
            .Where(p => p.StockQuantity <= threshold)
            .OrderBy(p => p.StockQuantity)
            .ToList();
    }
}
