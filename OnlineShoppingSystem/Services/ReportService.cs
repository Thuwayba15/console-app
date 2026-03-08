using OnlineShoppingSystem.Data;
using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Enums;

namespace OnlineShoppingSystem.Services;

/// <summary>
/// Service for generating various reports
/// </summary>
public class ReportService : IReportService
{
    private readonly AppDataStore _dataStore;

    public ReportService()
    {
        _dataStore = AppDataStore.Instance;
    }

    public void GenerateSalesReport()
    {
        var orders = _dataStore.Orders;

        if (!orders.Any())
        {
            Console.WriteLine("\nNo orders found.");
            return;
        }

        var totalRevenue = orders.Sum(o => o.TotalAmount);
        var totalOrders = orders.Count;
        var deliveredOrders = orders.Count(o => o.Status == OrderStatus.Delivered);
        var pendingOrders = orders.Count(o => o.Status == OrderStatus.Pending);

        Console.WriteLine("\n========== SALES REPORT ==========");
        Console.WriteLine($"Total Orders: {totalOrders}");
        Console.WriteLine($"Total Revenue: ${totalRevenue:F2}");
        Console.WriteLine($"Delivered Orders: {deliveredOrders}");
        Console.WriteLine($"Pending Orders: {pendingOrders}");

        // Top selling products
        var topProducts = orders
            .SelectMany(o => o.Items)
            .GroupBy(item => item.ProductName)
            .Select(g => new
            {
                ProductName = g.Key,
                TotalQuantity = g.Sum(item => item.Quantity),
                TotalRevenue = g.Sum(item => item.Subtotal)
            })
            .OrderByDescending(x => x.TotalRevenue)
            .Take(5);

        Console.WriteLine("\nTop 5 Products by Revenue:");
        foreach (var product in topProducts)
        {
            Console.WriteLine($"  {product.ProductName}: {product.TotalQuantity} sold, ${product.TotalRevenue:F2} revenue");
        }

        Console.WriteLine("==================================\n");
    }

    public void GenerateProductReviewsReport()
    {
        var reviews = _dataStore.Reviews;

        if (!reviews.Any())
        {
            Console.WriteLine("\nNo reviews found.");
            return;
        }

        var productsWithReviews = reviews
            .GroupBy(r => r.ProductId)
            .Select(g => new
            {
                ProductId = g.Key,
                ProductName = _dataStore.Products.FirstOrDefault(p => p.Id == g.Key)?.Name ?? "Unknown",
                ReviewCount = g.Count(),
                AverageRating = g.Average(r => r.Rating)
            })
            .OrderByDescending(x => x.AverageRating)
            .ThenByDescending(x => x.ReviewCount);

        Console.WriteLine("\n========== PRODUCT REVIEWS REPORT ==========");
        Console.WriteLine($"Total Reviews: {reviews.Count}");
        Console.WriteLine("\nProducts with Reviews:");

        foreach (var product in productsWithReviews)
        {
            Console.WriteLine($"  {product.ProductName}:");
            Console.WriteLine($"    Reviews: {product.ReviewCount}");
            Console.WriteLine($"    Average Rating: {product.AverageRating:F1}/5.0");
        }

        Console.WriteLine("============================================\n");
    }
}
