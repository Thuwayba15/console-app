using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Helpers;

/// <summary>
/// Helper class for displaying sales reports
/// </summary>
public static class ReportDisplayHelper
{
    /// <summary>
    /// Display sales summary with aggregated statistics
    /// </summary>
    public static void DisplaySalesSummary(List<Order> orders)
    {
        if (!orders.Any())
        {
            ConsoleHelper.DisplayWarning("No orders found.");
            return;
        }

        var totalOrders = orders.Count;
        var totalRevenue = orders.Sum(o => o.TotalAmount);
        var totalItemsSold = orders.SelectMany(o => o.Items).Sum(i => i.Quantity);
        var averageOrderValue = totalRevenue / totalOrders;

        Console.WriteLine($"Total Orders: {totalOrders}");
        Console.WriteLine($"Total Revenue: R{totalRevenue:F2}");
        Console.WriteLine($"Total Items Sold: {totalItemsSold}");
        Console.WriteLine($"Average Order Value: R{averageOrderValue:F2}");
        
        Console.WriteLine("\nOrders by Status:");
        var ordersByStatus = orders.GroupBy(o => o.Status);
        foreach (var group in ordersByStatus.OrderBy(g => g.Key))
        {
            Console.WriteLine($"  {group.Key}: {group.Count()}");
        }
    }

    /// <summary>
    /// Display top-selling products in a ranked table
    /// </summary>
    public static void DisplayTopProducts(List<Order> orders, int limit)
    {
        if (!orders.Any())
        {
            ConsoleHelper.DisplayWarning("No sales data available.");
            return;
        }

        var topProducts = orders
            .SelectMany(o => o.Items)
            .GroupBy(i => new { i.ProductId, i.ProductName })
            .Select(g => new
            {
                ProductName = g.Key.ProductName,
                QuantitySold = g.Sum(i => i.Quantity),
                Revenue = g.Sum(i => i.Subtotal)
            })
            .OrderByDescending(x => x.QuantitySold)
            .Take(limit);

        Console.WriteLine("\n{0,-5} {1,-30} {2,-15} {3,-12}", "Rank", "Product", "Quantity Sold", "Revenue");
        Console.WriteLine(new string('-', 65));

        var rank = 1;
        foreach (var item in topProducts)
        {
            var displayName = item.ProductName.Length > 28 
                ? item.ProductName.Substring(0, 28) + ".." 
                : item.ProductName;

            Console.WriteLine("{0,-5} {1,-30} {2,-15} R{3,-11:F2}",
                rank++,
                displayName,
                item.QuantitySold,
                item.Revenue);
        }
    }

    /// <summary>
    /// Display sales grouped by product category
    /// </summary>
    public static void DisplaySalesByCategory(List<Order> orders, List<Product> products)
    {
        if (!orders.Any())
        {
            ConsoleHelper.DisplayWarning("No sales data available.");
            return;
        }

        var orderItems = orders.SelectMany(o => o.Items).ToList();

        var salesByCategory = orderItems
            .Join(products,
                item => item.ProductId,
                product => product.Id,
                (item, product) => new { item, product })
            .GroupBy(x => x.product.Category)
            .Select(g => new
            {
                Category = g.Key,
                TotalQuantitySold = g.Sum(x => x.item.Quantity),
                TotalRevenue = g.Sum(x => x.item.Subtotal)
            })
            .OrderByDescending(x => x.TotalRevenue);

        Console.WriteLine("\n{0,-20} {1,-15} {2,-12}", "Category", "Items Sold", "Revenue");
        Console.WriteLine(new string('-', 50));

        foreach (var item in salesByCategory)
        {
            Console.WriteLine("{0,-20} {1,-15} R{2,-11:F2}",
                item.Category,
                item.TotalQuantitySold,
                item.TotalRevenue);
        }

        var totalRevenue = salesByCategory.Sum(s => s.TotalRevenue);
        Console.WriteLine(new string('-', 50));
        Console.WriteLine("{0,-35} R{1,-11:F2}", "TOTAL:", totalRevenue);
    }
}
