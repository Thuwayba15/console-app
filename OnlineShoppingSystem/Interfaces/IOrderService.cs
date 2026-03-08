using OnlineShoppingSystem.Models;
using OnlineShoppingSystem.Enums;

namespace OnlineShoppingSystem.Interfaces;

/// <summary>
/// Interface for order management operations
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Create an order from a customer's cart
    /// </summary>
    Order? CreateOrder(int customerId);

    /// <summary>
    /// Get all orders for a customer
    /// </summary>
    List<Order> GetCustomerOrders(int customerId);

    /// <summary>
    /// Get an order by ID
    /// </summary>
    Order? GetOrderById(int orderId);

    /// <summary>
    /// Get all orders (admin only)
    /// </summary>
    List<Order> GetAllOrders();

    /// <summary>
    /// Update the status of an order (admin only)
    /// </summary>
    bool UpdateOrderStatus(int orderId, OrderStatus newStatus);
}
