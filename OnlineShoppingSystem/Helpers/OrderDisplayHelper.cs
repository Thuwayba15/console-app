using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Helpers;

/// <summary>
/// Helper class for displaying order and cart information
/// </summary>
public static class OrderDisplayHelper
{
    /// <summary>
    /// Display cart contents in a formatted table
    /// </summary>
    public static void DisplayCart(Cart cart)
    {
        Console.WriteLine("\n{0,-5} {1,-30} {2,-12} {3,-10} {4,-12}", 
            "ID", "Product", "Price", "Quantity", "Subtotal");
        Console.WriteLine(new string('-', 75));

        foreach (var item in cart.Items)
        {
            var nameDisplay = item.ProductName.Length > 28 
                ? item.ProductName.Substring(0, 28) + ".." 
                : item.ProductName;

            Console.WriteLine("{0,-5} {1,-30} R{2,-11:F2} {3,-10} R{4,-11:F2}",
                item.ProductId,
                nameDisplay,
                item.Price,
                item.Quantity,
                item.Subtotal);
        }

        Console.WriteLine(new string('-', 75));
        Console.WriteLine("{0,-58} R{1,-11:F2}\n", "TOTAL:", cart.GetTotal());
    }

    /// <summary>
    /// Display order summary for checkout
    /// </summary>
    public static void DisplayCheckoutSummary(Cart cart)
    {
        Console.WriteLine("\n=== ORDER SUMMARY ===");
        foreach (var item in cart.Items)
        {
            Console.WriteLine($"{item.Quantity}x {item.ProductName} @ R{item.Price:F2} = R{item.Subtotal:F2}");
        }
        Console.WriteLine(new string('-', 50));
        Console.WriteLine($"TOTAL: R{cart.GetTotal():F2}");
    }

    /// <summary>
    /// Display order history for a customer
    /// </summary>
    public static void DisplayOrderHistory(List<Order> orders)
    {
        foreach (var order in orders)
        {
            Console.WriteLine($"\n--- Order #{order.Id} ---");
            Console.WriteLine($"Date: {order.OrderDate:yyyy-MM-dd HH:mm}");
            Console.WriteLine($"Status: {order.Status}");
            Console.WriteLine($"Total: R{order.TotalAmount:F2}");
            Console.WriteLine("Items:");
            
            foreach (var item in order.Items)
            {
                Console.WriteLine($"  - {item.Quantity}x {item.ProductName} @ R{item.Price:F2} = R{item.Subtotal:F2}");
            }

            if (order.DeliveryDate.HasValue)
            {
                Console.WriteLine($"Delivered: {order.DeliveryDate.Value:yyyy-MM-dd HH:mm}");
            }
        }
        Console.WriteLine();
    }

    /// <summary>
    /// Display order tracking summary table
    /// </summary>
    public static void DisplayOrderSummaryTable(List<Order> orders)
    {
        Console.WriteLine("\n{0,-10} {1,-20} {2,-15} {3,-12}", "Order ID", "Date", "Status", "Total");
        Console.WriteLine(new string('-', 60));

        foreach (var order in orders)
        {
            Console.WriteLine("{0,-10} {1,-20} {2,-15} R{3,-11:F2}",
                order.Id,
                order.OrderDate.ToString("yyyy-MM-dd HH:mm"),
                order.Status,
                order.TotalAmount);
        }
        Console.WriteLine();
    }

    /// <summary>
    /// Display detailed order information
    /// </summary>
    public static void DisplayOrderDetails(Order order)
    {
        Console.WriteLine($"\n=== ORDER #{order.Id} DETAILS ===");
        Console.WriteLine($"Order Date: {order.OrderDate:yyyy-MM-dd HH:mm}");
        Console.WriteLine($"Status: {order.Status}");
        Console.WriteLine($"Total Amount: R{order.TotalAmount:F2}");
        
        if (order.DeliveryDate.HasValue)
        {
            Console.WriteLine($"Delivery Date: {order.DeliveryDate.Value:yyyy-MM-dd HH:mm}");
        }

        Console.WriteLine("\nOrder Items:");
        foreach (var item in order.Items)
        {
            Console.WriteLine($"  {item.Quantity}x {item.ProductName} @ R{item.Price:F2} = R{item.Subtotal:F2}");
        }
        Console.WriteLine();
    }
}
