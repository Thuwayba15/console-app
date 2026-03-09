using OnlineShoppingSystem.Data;
using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Models;
using OnlineShoppingSystem.Validators;

namespace OnlineShoppingSystem.Services;

/// <summary>
/// Service for managing products
/// Uses ProductValidator for consistent validation
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
        // Use ProductValidator for consistent validation
        var validation = ProductValidator.ValidateProduct(name, description, price, category, stockQuantity);
        if (!validation.IsValid)
        {
            throw new ArgumentException(validation.ErrorMessage);
        }

        var product = new Product
        {
            Id = _dataStore.GetNextProductId(),
            Name = name.Trim(),
            Description = description?.Trim() ?? string.Empty,
            Price = price,
            StockQuantity = stockQuantity,
            Category = category?.Trim() ?? string.Empty
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

        // Use ProductValidator for consistent validation
        var validation = ProductValidator.ValidateProduct(name, description, price, category, stockQuantity);
        if (!validation.IsValid)
        {
            throw new ArgumentException(validation.ErrorMessage);
        }

        product.Name = name.Trim();
        product.Description = description?.Trim() ?? string.Empty;
        product.Price = price;
        product.StockQuantity = stockQuantity;
        product.Category = category?.Trim() ?? string.Empty;

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
