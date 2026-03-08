namespace OnlineShoppingSystem.Models;

/// <summary>
/// Represents a shopping cart for a customer
/// </summary>
public class Cart
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public List<CartItem> Items { get; set; }
    public DateTime CreatedAt { get; set; }

    public Cart()
    {
        Items = new List<CartItem>();
        CreatedAt = DateTime.Now;
    }

    /// <summary>
    /// Calculate the total cost of all items in the cart
    /// </summary>
    public decimal GetTotal()
    {
        return Items.Sum(item => item.Subtotal);
    }
}
