namespace OnlineShoppingSystem.Models;

/// <summary>
/// Represents a single item in a shopping cart
/// </summary>
public class CartItem
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Subtotal => Price * Quantity;

    public CartItem()
    {
        ProductName = string.Empty;
    }
}
