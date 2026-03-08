using OnlineShoppingSystem.Data;
using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Services;

/// <summary>
/// Service for managing shopping cart operations
/// </summary>
public class CartService : ICartService
{
    private readonly AppDataStore _dataStore;
    private readonly IProductService _productService;

    public CartService(IProductService productService)
    {
        _dataStore = AppDataStore.Instance;
        _productService = productService;
    }

    public Cart? GetCart(int customerId)
    {
        return _dataStore.Carts.FirstOrDefault(c => c.CustomerId == customerId);
    }

    public bool AddToCart(int customerId, int productId, int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero.");
        }

        var cart = GetCart(customerId);
        if (cart == null)
        {
            return false;
        }

        var product = _productService.GetProductById(productId);
        if (product == null)
        {
            throw new InvalidOperationException("Product not found.");
        }

        if (product.StockQuantity < quantity)
        {
            throw new InvalidOperationException($"Insufficient stock. Only {product.StockQuantity} items available.");
        }

        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (existingItem != null)
        {
            var newQuantity = existingItem.Quantity + quantity;
            if (product.StockQuantity < newQuantity)
            {
                throw new InvalidOperationException($"Insufficient stock. Only {product.StockQuantity} items available.");
            }
            existingItem.Quantity = newQuantity;
        }
        else
        {
            var cartItem = new CartItem
            {
                ProductId = productId,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = quantity
            };
            cart.Items.Add(cartItem);
        }

        return true;
    }

    public bool UpdateCartItem(int customerId, int productId, int newQuantity)
    {
        if (newQuantity < 0)
        {
            throw new ArgumentException("Quantity cannot be negative.");
        }

        var cart = GetCart(customerId);
        if (cart == null)
        {
            return false;
        }

        var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (cartItem == null)
        {
            return false;
        }

        if (newQuantity == 0)
        {
            cart.Items.Remove(cartItem);
            return true;
        }

        var product = _productService.GetProductById(productId);
        if (product == null)
        {
            throw new InvalidOperationException("Product not found.");
        }

        if (product.StockQuantity < newQuantity)
        {
            throw new InvalidOperationException($"Insufficient stock. Only {product.StockQuantity} items available.");
        }

        cartItem.Quantity = newQuantity;
        return true;
    }

    public bool RemoveFromCart(int customerId, int productId)
    {
        var cart = GetCart(customerId);
        if (cart == null)
        {
            return false;
        }

        var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (cartItem == null)
        {
            return false;
        }

        cart.Items.Remove(cartItem);
        return true;
    }

    public void ClearCart(int customerId)
    {
        var cart = GetCart(customerId);
        if (cart != null)
        {
            cart.Items.Clear();
        }
    }
}
