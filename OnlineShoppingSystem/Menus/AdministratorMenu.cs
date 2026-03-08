using OnlineShoppingSystem.Enums;
using OnlineShoppingSystem.Helpers;
using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Menus;

/// <summary>
/// Administrator menu for managing products, orders, and reports
/// </summary>
public class AdministratorMenu
{
    private readonly Administrator _admin;
    private readonly IProductService _productService;
    private readonly IOrderService _orderService;
    private readonly IReportService _reportService;
    private readonly IPersistenceService _persistenceService;

    public AdministratorMenu(
        Administrator admin,
        IProductService productService,
        IOrderService orderService,
        IReportService reportService,
        IPersistenceService persistenceService)
    {
        _admin = admin;
        _productService = productService;
        _orderService = orderService;
        _reportService = reportService;
        _persistenceService = persistenceService;
    }

    /// <summary>
    /// Display and handle the administrator menu
    /// </summary>
    public void Show()
    {
        while (true)
        {
            ConsoleHelper.DisplayHeader($"ADMINISTRATOR MENU - Welcome, {_admin.Username}!");
            Console.WriteLine("1. Add Product");
            Console.WriteLine("2. Update Product");
            Console.WriteLine("3. Delete Product");
            Console.WriteLine("4. Restock Product");
            Console.WriteLine("5. View Products");
            Console.WriteLine("6. View Orders");
            Console.WriteLine("7. Update Order Status");
            Console.WriteLine("8. View Low Stock Products");
            Console.WriteLine("9. Generate Sales Reports");
            Console.WriteLine("10. Logout");

            var choice = InputHelper.ReadMenuChoice(1, 10);

            switch (choice)
            {
                case 1:
                    AddProduct();
                    break;
                case 2:
                    UpdateProduct();
                    break;
                case 3:
                    DeleteProduct();
                    break;
                case 4:
                    RestockProduct();
                    break;
                case 5:
                    ViewProducts();
                    break;
                case 6:
                    ViewOrders();
                    break;
                case 7:
                    UpdateOrderStatus();
                    break;
                case 8:
                    ViewLowStockProducts();
                    break;
                case 9:
                    GenerateSalesReports();
                    break;
                case 10:
                    ConsoleHelper.DisplaySuccess("Logged out successfully.");
                    ConsoleHelper.PauseForUser();
                    return;
            }
        }
    }

    #region Product Management

    /// <summary>
    /// Add a new product to the catalog
    /// </summary>
    private void AddProduct()
    {
        ConsoleHelper.DisplayHeader("ADD NEW PRODUCT");

        try
        {
            // Get product details
            var name = InputHelper.ReadNonEmptyString("Enter product name: ");
            var description = InputHelper.ReadNonEmptyString("Enter product description: ");
            var price = InputHelper.ReadPositiveDecimal("Enter product price: R");
            var stockQuantity = InputHelper.ReadPositiveInt("Enter initial stock quantity: ");
            var category = InputHelper.ReadNonEmptyString("Enter product category: ");

            // Create product
            var product = _productService.AddProduct(name, description, price, stockQuantity, category);

            if (product != null)
            {
                ConsoleHelper.DisplaySuccess($"Product '{product.Name}' added successfully!");
                ConsoleHelper.DisplayInfo($"Product ID: {product.Id}");
                ConsoleHelper.DisplayInfo($"Price: R{product.Price:F2}");
                ConsoleHelper.DisplayInfo($"Stock: {product.StockQuantity}");
                
                // Save data immediately
                _persistenceService.SaveData();
            }
            else
            {
                ConsoleHelper.DisplayError("Failed to add product.");
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.DisplayError($"Error adding product: {ex.Message}");
        }

        ConsoleHelper.PauseForUser();
    }

    /// <summary>
    /// Update an existing product's details
    /// </summary>
    private void UpdateProduct()
    {
        ConsoleHelper.DisplayHeader("UPDATE PRODUCT");

        try
        {
            // Display all products
            var products = _productService.GetAllProducts();
            if (!products.Any())
            {
                ConsoleHelper.DisplayWarning("No products available.");
                ConsoleHelper.PauseForUser();
                return;
            }

            ProductDisplayHelper.DisplayProductTable(products);

            // Get product to update
            var productId = InputHelper.ReadInt("Enter Product ID to update (0 to cancel): ");
            if (productId == 0) return;

            var product = _productService.GetProductById(productId);
            if (product == null)
            {
                ConsoleHelper.DisplayError("Product not found.");
                ConsoleHelper.PauseForUser();
                return;
            }

            // Display current details
            Console.WriteLine($"\nCurrent details for: {product.Name}");
            Console.WriteLine($"Price: R{product.Price:F2}");
            Console.WriteLine($"Stock: {product.StockQuantity}");
            Console.WriteLine($"Category: {product.Category}");
            Console.WriteLine($"Description: {product.Description}");

            // Get new values (press Enter to keep current)
            Console.WriteLine("\nEnter new values (press Enter to keep current):");
            
            var newName = InputHelper.ReadString($"Name [{product.Name}]: ");
            if (string.IsNullOrWhiteSpace(newName)) newName = product.Name;

            var newDescription = InputHelper.ReadString($"Description [{product.Description}]: ");
            if (string.IsNullOrWhiteSpace(newDescription)) newDescription = product.Description;

            var priceInput = InputHelper.ReadString($"Price [R{product.Price:F2}]: ");
            var newPrice = string.IsNullOrWhiteSpace(priceInput) ? product.Price : decimal.Parse(priceInput);

            var categoryInput = InputHelper.ReadString($"Category [{product.Category}]: ");
            var newCategory = string.IsNullOrWhiteSpace(categoryInput) ? product.Category : categoryInput;

            // Update product
            var success = _productService.UpdateProduct(productId, newName, newDescription, newPrice, product.StockQuantity, newCategory);

            if (success)
            {
                ConsoleHelper.DisplaySuccess("Product updated successfully!");
                
                // Save data immediately
                _persistenceService.SaveData();
            }
            else
            {
                ConsoleHelper.DisplayError("Failed to update product.");
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.DisplayError($"Error updating product: {ex.Message}");
        }

        ConsoleHelper.PauseForUser();
    }

    /// <summary>
    /// Delete a product from the catalog
    /// </summary>
    private void DeleteProduct()
    {
        ConsoleHelper.DisplayHeader("DELETE PRODUCT");

        try
        {
            // Display all products
            var products = _productService.GetAllProducts();
            if (!products.Any())
            {
                ConsoleHelper.DisplayWarning("No products available.");
                ConsoleHelper.PauseForUser();
                return;
            }

            ProductDisplayHelper.DisplayProductTable(products);

            // Get product to delete
            var productId = InputHelper.ReadInt("Enter Product ID to delete (0 to cancel): ");
            if (productId == 0) return;

            var product = _productService.GetProductById(productId);
            if (product == null)
            {
                ConsoleHelper.DisplayError("Product not found.");
                ConsoleHelper.PauseForUser();
                return;
            }

            // Confirm deletion
            Console.Write($"\nAre you sure you want to delete '{product.Name}'? (yes/no): ");
            var confirmation = Console.ReadLine()?.Trim().ToLower();

            if (confirmation != "yes" && confirmation != "y")
            {
                ConsoleHelper.DisplayInfo("Deletion cancelled.");
                ConsoleHelper.PauseForUser();
                return;
            }

            // Delete product
            var success = _productService.DeleteProduct(productId);

            if (success)
            {
                ConsoleHelper.DisplaySuccess($"Product '{product.Name}' deleted successfully!");
                
                // Save data immediately
                _persistenceService.SaveData();
            }
            else
            {
                ConsoleHelper.DisplayError("Failed to delete product.");
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.DisplayError($"Error deleting product: {ex.Message}");
        }

        ConsoleHelper.PauseForUser();
    }

    /// <summary>
    /// Restock a product by adding to its inventory
    /// </summary>
    private void RestockProduct()
    {
        ConsoleHelper.DisplayHeader("RESTOCK PRODUCT");

        try
        {
            // Display all products
            var products = _productService.GetAllProducts();
            if (!products.Any())
            {
                ConsoleHelper.DisplayWarning("No products available.");
                ConsoleHelper.PauseForUser();
                return;
            }

            ProductDisplayHelper.DisplayProductTable(products);

            // Get product to restock
            var productId = InputHelper.ReadInt("Enter Product ID to restock (0 to cancel): ");
            if (productId == 0) return;

            var product = _productService.GetProductById(productId);
            if (product == null)
            {
                ConsoleHelper.DisplayError("Product not found.");
                ConsoleHelper.PauseForUser();
                return;
            }

            Console.WriteLine($"\nProduct: {product.Name}");
            Console.WriteLine($"Current Stock: {product.StockQuantity}");

            var quantity = InputHelper.ReadPositiveInt("Enter quantity to add: ");

            // Restock product
            var success = _productService.RestockProduct(productId, quantity);

            if (success)
            {
                var updatedProduct = _productService.GetProductById(productId);
                ConsoleHelper.DisplaySuccess($"Product restocked successfully!");
                ConsoleHelper.DisplayInfo($"New stock level: {updatedProduct?.StockQuantity}");
                
                // Save data immediately
                _persistenceService.SaveData();
            }
            else
            {
                ConsoleHelper.DisplayError("Failed to restock product.");
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.DisplayError($"Error restocking product: {ex.Message}");
        }

        ConsoleHelper.PauseForUser();
    }

    /// <summary>
    /// View all products in the catalog
    /// </summary>
    private void ViewProducts()
    {
        ConsoleHelper.DisplayHeader("ALL PRODUCTS");

        var products = _productService.GetAllProducts();

        if (!products.Any())
        {
            ConsoleHelper.DisplayWarning("No products available.");
            ConsoleHelper.PauseForUser();
            return;
        }

        ProductDisplayHelper.DisplayProductTable(products);
        ConsoleHelper.DisplayInfo($"Total products: {products.Count}");
        ConsoleHelper.PauseForUser();
    }

    /// <summary>
    /// View products with low stock levels
    /// </summary>
    private void ViewLowStockProducts()
    {
        ConsoleHelper.DisplayHeader("LOW STOCK PRODUCTS");

        var lowStockProducts = _productService.GetLowStockProducts(10);  // 10 units threshold

        if (!lowStockProducts.Any())
        {
            ConsoleHelper.DisplaySuccess("All products are adequately stocked!");
            ConsoleHelper.PauseForUser();
            return;
        }

        ConsoleHelper.DisplayWarning($"Found {lowStockProducts.Count} product(s) with low stock (?10 units):");
        ProductDisplayHelper.DisplayProductTable(lowStockProducts);
        ConsoleHelper.PauseForUser();
    }

    #endregion

    #region Order Management

    /// <summary>
    /// View all orders in the system
    /// </summary>
    private void ViewOrders()
    {
        ConsoleHelper.DisplayHeader("ALL ORDERS");

        var orders = _orderService.GetAllOrders();

        if (!orders.Any())
        {
            ConsoleHelper.DisplayWarning("No orders found.");
            ConsoleHelper.PauseForUser();
            return;
        }

        // Display orders grouped by status
        var groupedOrders = orders.GroupBy(o => o.Status).OrderBy(g => g.Key);

        foreach (var group in groupedOrders)
        {
            Console.WriteLine($"\n=== {group.Key} Orders ({group.Count()}) ===");
            Console.WriteLine("{0,-10} {1,-15} {2,-20} {3,-12} {4,-10}", 
                "Order ID", "Customer ID", "Date", "Total", "Items");
            Console.WriteLine(new string('-', 70));

            foreach (var order in group.OrderBy(o => o.OrderDate))
            {
                Console.WriteLine("{0,-10} {1,-15} {2,-20} R{3,-11:F2} {4,-10}",
                    order.Id,
                    order.CustomerId,
                    order.OrderDate.ToString("yyyy-MM-dd HH:mm"),
                    order.TotalAmount,
                    order.Items.Count);
            }
        }

        Console.WriteLine($"\nTotal orders: {orders.Count}");
        ConsoleHelper.PauseForUser();
    }

    /// <summary>
    /// Update the status of an order
    /// </summary>
    private void UpdateOrderStatus()
    {
        ConsoleHelper.DisplayHeader("UPDATE ORDER STATUS");

        try
        {
            var orders = _orderService.GetAllOrders();

            if (!orders.Any())
            {
                ConsoleHelper.DisplayWarning("No orders found.");
                ConsoleHelper.PauseForUser();
                return;
            }

            // Display orders
            Console.WriteLine("{0,-10} {1,-15} {2,-20} {3,-15}", 
                "Order ID", "Customer ID", "Date", "Status");
            Console.WriteLine(new string('-', 65));

            foreach (var order in orders.OrderByDescending(o => o.OrderDate))
            {
                Console.WriteLine("{0,-10} {1,-15} {2,-20} {3,-15}",
                    order.Id,
                    order.CustomerId,
                    order.OrderDate.ToString("yyyy-MM-dd HH:mm"),
                    order.Status);
            }

            Console.WriteLine();

            // Get order to update
            var orderId = InputHelper.ReadInt("Enter Order ID to update (0 to cancel): ");
            if (orderId == 0) return;

            var selectedOrder = _orderService.GetOrderById(orderId);
            if (selectedOrder == null)
            {
                ConsoleHelper.DisplayError("Order not found.");
                ConsoleHelper.PauseForUser();
                return;
            }

            Console.WriteLine($"\nOrder #{selectedOrder.Id}");
            Console.WriteLine($"Current Status: {selectedOrder.Status}");
            Console.WriteLine($"Customer ID: {selectedOrder.CustomerId}");
            Console.WriteLine($"Total: R{selectedOrder.TotalAmount:F2}");

            // Display status options
            Console.WriteLine("\nSelect new status:");
            Console.WriteLine("1. Pending");
            Console.WriteLine("2. Processing");
            Console.WriteLine("3. Shipped");
            Console.WriteLine("4. Delivered");
            Console.WriteLine("5. Cancelled");

            var statusChoice = InputHelper.ReadMenuChoice(1, 5);

            var newStatus = statusChoice switch
            {
                1 => OrderStatus.Pending,
                2 => OrderStatus.Processing,
                3 => OrderStatus.Shipped,
                4 => OrderStatus.Delivered,
                5 => OrderStatus.Cancelled,
                _ => OrderStatus.Pending
            };

            // Update status
            var success = _orderService.UpdateOrderStatus(orderId, newStatus);

            if (success)
            {
                ConsoleHelper.DisplaySuccess($"Order #{orderId} status updated to {newStatus}!");
                
                if (newStatus == OrderStatus.Delivered)
                {
                    ConsoleHelper.DisplayInfo("Delivery date has been set.");
                }
                
                // Save data immediately
                _persistenceService.SaveData();
            }
            else
            {
                ConsoleHelper.DisplayError("Failed to update order status.");
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.DisplayError($"Error updating order status: {ex.Message}");
        }

        ConsoleHelper.PauseForUser();
    }

    #endregion

    #region Reports

    /// <summary>
    /// Generate and display sales reports
    /// </summary>
    private void GenerateSalesReports()
    {
        ConsoleHelper.DisplayHeader("SALES REPORTS");

        try
        {
            Console.WriteLine("Select report type:");
            Console.WriteLine("1. Sales Summary");
            Console.WriteLine("2. Top Products");
            Console.WriteLine("3. Sales by Category");
            Console.WriteLine("4. Back to Menu");

            var choice = InputHelper.ReadMenuChoice(1, 4);

            switch (choice)
            {
                case 1:
                    DisplaySalesSummary();
                    break;
                case 2:
                    DisplayTopProducts();
                    break;
                case 3:
                    DisplaySalesByCategory();
                    break;
                case 4:
                    return;
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.DisplayError($"Error generating report: {ex.Message}");
        }

        ConsoleHelper.PauseForUser();
    }

    /// <summary>
    /// Display overall sales summary
    /// </summary>
    private void DisplaySalesSummary()
    {
        ConsoleHelper.DisplayHeader("SALES SUMMARY");

        var orders = _orderService.GetAllOrders();
        
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
    /// Display top-selling products
    /// </summary>
    private void DisplayTopProducts()
    {
        ConsoleHelper.DisplayHeader("TOP SELLING PRODUCTS");

        var limit = InputHelper.ReadPositiveInt("Enter number of top products to display: ");
        var orders = _orderService.GetAllOrders();
        
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
            Console.WriteLine("{0,-5} {1,-30} {2,-15} R{3,-11:F2}",
                rank++,
                item.ProductName.Length > 28 ? item.ProductName.Substring(0, 28) + ".." : item.ProductName,
                item.QuantitySold,
                item.Revenue);
        }
    }

    /// <summary>
    /// Display sales grouped by category
    /// </summary>
    private void DisplaySalesByCategory()
    {
        ConsoleHelper.DisplayHeader("SALES BY CATEGORY");

        var orders = _orderService.GetAllOrders();
        var products = _productService.GetAllProducts();
        
        if (!orders.Any())
        {
            ConsoleHelper.DisplayWarning("No sales data available.");
            return;
        }

        // Get all order items with product details
        var orderItems = orders
            .SelectMany(o => o.Items)
            .ToList();

        // Group by category
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

    #endregion
}
