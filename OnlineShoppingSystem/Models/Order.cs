using OnlineShoppingSystem.Enums;

namespace OnlineShoppingSystem.Models;

/// <summary>
/// Represents a customer order
/// </summary>
public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public List<OrderItem> Items { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? DeliveryDate { get; set; }

    public Order()
    {
        Items = new List<OrderItem>();
        Status = OrderStatus.Pending;
        OrderDate = DateTime.Now;
    }
}
