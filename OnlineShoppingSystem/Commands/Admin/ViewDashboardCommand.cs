using OnlineShoppingSystem.Data;
using OnlineShoppingSystem.Enums;
using OnlineShoppingSystem.Helpers;
using OnlineShoppingSystem.Interfaces;

namespace OnlineShoppingSystem.Commands.Admin;

/// <summary>
/// Command for displaying admin dashboard with key statistics
/// Shows overview of system health and business metrics
/// </summary>
public class ViewDashboardCommand : ICommand
{
    private readonly IProductService _productService;
    private readonly IOrderService _orderService;
    private readonly AppDataStore _dataStore;

    public ViewDashboardCommand(IProductService productService, IOrderService orderService)
    {
        _productService = productService;
        _orderService = orderService;
        _dataStore = AppDataStore.Instance;
    }

    public void Execute()
    {
        ConsoleHelper.DisplayHeader("ADMIN DASHBOARD");

        try
        {
            // Get data
            var allUsers = _dataStore.Users;
            var allProducts = _productService.GetAllProducts();
            var allOrders = _orderService.GetAllOrders();
            var lowStockProducts = _productService.GetLowStockProducts(10);

            // Calculate statistics
            var totalCustomers = allUsers.Count(u => u.Role == Enums.UserRole.Customer);
            var totalAdmins = allUsers.Count(u => u.Role == Enums.UserRole.Administrator);
            var totalRevenue = allOrders.Where(o => o.Status != OrderStatus.Cancelled).Sum(o => o.TotalAmount);
            var pendingOrders = allOrders.Count(o => o.Status == OrderStatus.Pending);
            var processingOrders = allOrders.Count(o => o.Status == OrderStatus.Processing);
            var deliveredOrders = allOrders.Count(o => o.Status == OrderStatus.Delivered);
            var outOfStockProducts = allProducts.Count(p => p.StockQuantity == 0);
            var totalProductsSold = allOrders.Where(o => o.Status != OrderStatus.Cancelled)
                                             .SelectMany(o => o.Items)
                                             .Sum(i => i.Quantity);

            // Display System Overview
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n" + new string('=', 62));
            Console.WriteLine("                    SYSTEM OVERVIEW                         ");
            Console.WriteLine(new string('=', 62));
            Console.ResetColor();
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n[USERS]");
            Console.ResetColor();
            Console.WriteLine($"   Total Customers:      {totalCustomers}");
            Console.WriteLine($"   Total Administrators: {totalAdmins}");
            Console.WriteLine($"   Total Users:          {allUsers.Count}");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\n[PRODUCTS]");
            Console.ResetColor();
            Console.WriteLine($"   Total Products:       {allProducts.Count}");
            Console.WriteLine($"   Low Stock Items:      {lowStockProducts.Count} (<=10 units)");
            Console.WriteLine($"   Out of Stock:         {outOfStockProducts}");
            
            if (outOfStockProducts > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"   >> WARNING: {outOfStockProducts} product(s) out of stock!");
                Console.ResetColor();
            }

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n[ORDERS]");
            Console.ResetColor();
            Console.WriteLine($"   Total Orders:         {allOrders.Count}");
            Console.WriteLine($"   Pending:              {pendingOrders}");
            Console.WriteLine($"   Processing:           {processingOrders}");
            Console.WriteLine($"   Delivered:            {deliveredOrders}");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n[FINANCIAL]");
            Console.ResetColor();
            Console.WriteLine($"   Total Revenue:        R{totalRevenue:N2}");
            Console.WriteLine($"   Products Sold:        {totalProductsSold} units");
            
            if (allOrders.Any())
            {
                var avgOrderValue = totalRevenue / allOrders.Count(o => o.Status != OrderStatus.Cancelled);
                Console.WriteLine($"   Avg Order Value:      R{avgOrderValue:N2}");
            }

            // Display Recent Activity
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n" + new string('=', 62));
            Console.WriteLine("                    RECENT ACTIVITY                         ");
            Console.WriteLine(new string('=', 62));
            Console.ResetColor();

            var recentOrders = allOrders.OrderByDescending(o => o.OrderDate).Take(5).ToList();
            
            if (recentOrders.Any())
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nLast 5 Orders:");
                Console.ResetColor();
                Console.WriteLine("{0,-10} {1,-15} {2,-12} {3,-15}", "Order ID", "Date", "Total", "Status");
                Console.WriteLine(new string('-', 55));
                
                foreach (var order in recentOrders)
                {
                    var statusColor = order.Status switch
                    {
                        OrderStatus.Delivered => ConsoleColor.Green,
                        OrderStatus.Cancelled => ConsoleColor.Red,
                        OrderStatus.Pending => ConsoleColor.Yellow,
                        _ => ConsoleColor.White
                    };

                    Console.Write($"{order.Id,-10} {order.OrderDate:yyyy-MM-dd}  R{order.TotalAmount,-10:N2} ");
                    Console.ForegroundColor = statusColor;
                    Console.WriteLine($"{order.Status}");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.WriteLine("\n   No orders yet.");
            }

            // Display Alerts
            if (lowStockProducts.Any() || pendingOrders > 0 || outOfStockProducts > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n" + new string('=', 62));
                Console.WriteLine("                        ALERTS                              ");
                Console.WriteLine(new string('=', 62));
                Console.ResetColor();

                if (outOfStockProducts > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n   [!] {outOfStockProducts} product(s) are OUT OF STOCK!");
                    Console.ResetColor();
                }

                if (lowStockProducts.Any())
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"   [!] {lowStockProducts.Count} product(s) have LOW STOCK (<=10 units)");
                    Console.ResetColor();
                }

                if (pendingOrders > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"   [!] {pendingOrders} order(s) need processing");
                    Console.ResetColor();
                }
            }

            // Top Products (if orders exist)
            if (allOrders.Any())
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n" + new string('=', 62));
                Console.WriteLine("                    TOP 5 PRODUCTS                          ");
                Console.WriteLine(new string('=', 62));
                Console.ResetColor();

                var topProducts = allOrders
                    .Where(o => o.Status != OrderStatus.Cancelled)
                    .SelectMany(o => o.Items)
                    .GroupBy(i => i.ProductId)
                    .Select(g => new
                    {
                        ProductId = g.Key,
                        ProductName = g.First().ProductName,
                        TotalQuantity = g.Sum(i => i.Quantity),
                        TotalRevenue = g.Sum(i => i.Quantity * i.Price)
                    })
                    .OrderByDescending(x => x.TotalRevenue)
                    .Take(5)
                    .ToList();

                if (topProducts.Any())
                {
                    Console.WriteLine("\n{0,-30} {1,-12} {2,-15}", "Product", "Sold", "Revenue");
                    Console.WriteLine(new string('-', 60));

                    int rank = 1;
                    foreach (var product in topProducts)
                    {
                        // Color top 3 differently
                        if (rank == 1) Console.ForegroundColor = ConsoleColor.Yellow;
                        else if (rank == 2) Console.ForegroundColor = ConsoleColor.Gray;
                        else if (rank == 3) Console.ForegroundColor = ConsoleColor.DarkYellow;
                        
                        Console.Write($"#{rank} ");
                        Console.ResetColor();
                        Console.WriteLine("{0,-27} {1,-12} R{2,-14:N2}",
                            product.ProductName.Length > 25 ? product.ProductName.Substring(0, 25) + ".." : product.ProductName,
                            product.TotalQuantity,
                            product.TotalRevenue);
                        rank++;
                    }
                }
            }

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\n" + new string('=', 62));
            Console.WriteLine($"Dashboard generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine(new string('=', 62));
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            ConsoleHelper.DisplayError($"Error loading dashboard: {ex.Message}");
        }

        ConsoleHelper.PauseForUser();
    }

    public string GetName() => "View Dashboard";
}
