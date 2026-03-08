using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Interfaces;

/// <summary>
/// Interface for shopping cart operations
/// </summary>
public interface ICartService
{
    /// <summary>
    /// Get a customer's cart
    /// </summary>
    Cart? GetCart(int customerId);

    /// <summary>
    /// Add a product to the cart
    /// </summary>
    bool AddToCart(int customerId, int productId, int quantity);

    /// <summary>
    /// Update the quantity of an item in the cart
    /// </summary>
    bool UpdateCartItem(int customerId, int productId, int newQuantity);

    /// <summary>
    /// Remove an item from the cart
    /// </summary>
    bool RemoveFromCart(int customerId, int productId);

    /// <summary>
    /// Clear all items from the cart
    /// </summary>
    void ClearCart(int customerId);
}
