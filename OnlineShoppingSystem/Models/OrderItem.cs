namespace OnlineShoppingSystem.Models;

/// <summary>
/// Represents a single item in an order
/// </summary>
public class OrderItem
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Subtotal => Price * Quantity;

    public OrderItem()
    {
        ProductName = string.Empty;
    }
}
