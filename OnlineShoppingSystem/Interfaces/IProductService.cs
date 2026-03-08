using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Interfaces;

/// <summary>
/// Interface for product management operations
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Get all products
    /// </summary>
    List<Product> GetAllProducts();

    /// <summary>
    /// Get a product by ID
    /// </summary>
    Product? GetProductById(int productId);

    /// <summary>
    /// Search products by name or description
    /// </summary>
    List<Product> SearchProducts(string searchTerm);

    /// <summary>
    /// Add a new product (admin only)
    /// </summary>
    Product AddProduct(string name, string description, decimal price, int stockQuantity, string category);

    /// <summary>
    /// Update an existing product (admin only)
    /// </summary>
    bool UpdateProduct(int productId, string name, string description, decimal price, int stockQuantity, string category);

    /// <summary>
    /// Delete a product (admin only)
    /// </summary>
    bool DeleteProduct(int productId);

    /// <summary>
    /// Restock a product (admin only)
    /// </summary>
    bool RestockProduct(int productId, int additionalQuantity);

    /// <summary>
    /// Get products with low stock
    /// </summary>
    List<Product> GetLowStockProducts(int threshold);
}
