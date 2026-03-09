using OnlineShoppingSystem.Enums;
using OnlineShoppingSystem.Helpers;
using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Models;
using OnlineShoppingSystem.Services;
using OnlineShoppingSystem.Strategies;

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
            var name = InputHelper.ReadNonEmptyString("Enter product name: ");
            if (name.Length > 100)
            {
                ConsoleHelper.DisplayError("Product name cannot exceed 100 characters.");
                ConsoleHelper.PauseForUser();
                return;
            }

            if (ValidationHelper.ContainsDangerousContent(name))
            {
                ConsoleHelper.DisplayError("Product name contains invalid characters or patterns.");
                ConsoleHelper.PauseForUser();
                return;
            }
            
            var description = InputHelper.ReadNonEmptyString("Enter product description: ");
            if (description.Length > 500)
            {
                ConsoleHelper.DisplayError("Description cannot exceed 500 characters.");
                ConsoleHelper.PauseForUser();
                return;
            }

            if (ValidationHelper.ContainsDangerousContent(description))
            {
                ConsoleHelper.DisplayError("Description contains invalid characters or patterns.");
                ConsoleHelper.PauseForUser();
                return;
            }
            
            var price = InputHelper.ReadPositiveDecimal("Enter product price: R");
            if (price > 1000000)
            {
                ConsoleHelper.DisplayError("Price cannot exceed R1,000,000.");
                ConsoleHelper.PauseForUser();
                return;
            }
            
            var stockQuantity = InputHelper.ReadPositiveInt("Enter initial stock quantity: ");
            if (stockQuantity > 1000000)
            {
                ConsoleHelper.DisplayError("Stock quantity cannot exceed 1,000,000 units.");
                ConsoleHelper.PauseForUser();
                return;
            }
            
            var category = InputHelper.ReadNonEmptyString("Enter product category: ");
            if (category.Length > 50)
            {
                ConsoleHelper.DisplayError("Category name cannot exceed 50 characters.");
                ConsoleHelper.PauseForUser();
                return;
            }

            if (ValidationHelper.ContainsDangerousContent(category))
            {
                ConsoleHelper.DisplayError("Category name contains invalid characters or patterns.");
                ConsoleHelper.PauseForUser();
                return;
            }

            var product = _productService.AddProduct(name, description, price, stockQuantity, category);

            if (product == null)
            {
                ConsoleHelper.DisplayError("Failed to add product.");
                ConsoleHelper.PauseForUser();
                return;
            }

            ConsoleHelper.DisplaySuccess($"Product '{product.Name}' added successfully!");
            ConsoleHelper.DisplayInfo($"Product ID: {product.Id}");
            ConsoleHelper.DisplayInfo($"Price: R{product.Price:F2}");
            ConsoleHelper.DisplayInfo($"Stock: {product.StockQuantity}");
            
            _persistenceService.SaveData();
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
            var products = _productService.GetAllProducts();
            if (!products.Any())
            {
                ConsoleHelper.DisplayWarning("No products available.");
                ConsoleHelper.PauseForUser();
                return;
            }

            ProductDisplayHelper.DisplayProductTable(products);

            var productId = InputHelper.ReadInt("Enter Product ID to update (0 to cancel): ");
            if (productId == 0) return;

            var product = _productService.GetProductById(productId);
            if (product == null)
            {
                ConsoleHelper.DisplayError("Product not found.");
                ConsoleHelper.PauseForUser();
                return;
            }

            DisplayCurrentProductDetails(product);
            var updatedDetails = GetUpdatedProductDetails(product);

            var success = _productService.UpdateProduct(
                productId, 
                updatedDetails.Name, 
                updatedDetails.Description, 
                updatedDetails.Price, 
                product.StockQuantity, 
                updatedDetails.Category);

            if (!success)
            {
                ConsoleHelper.DisplayError("Failed to update product.");
                ConsoleHelper.PauseForUser();
                return;
            }

            ConsoleHelper.DisplaySuccess("Product updated successfully!");
            _persistenceService.SaveData();
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
            var products = _productService.GetAllProducts();
            if (!products.Any())
            {
                ConsoleHelper.DisplayWarning("No products available.");
                ConsoleHelper.PauseForUser();
                return;
            }

            ProductDisplayHelper.DisplayProductTable(products);

            var productId = InputHelper.ReadInt("Enter Product ID to delete (0 to cancel): ");
            if (productId == 0) return;

            var product = _productService.GetProductById(productId);
            if (product == null)
            {
                ConsoleHelper.DisplayError("Product not found.");
                ConsoleHelper.PauseForUser();
                return;
            }

            if (!ConfirmDeletion(product.Name))
            {
                ConsoleHelper.DisplayInfo("Deletion cancelled.");
                ConsoleHelper.PauseForUser();
                return;
            }

            var success = _productService.DeleteProduct(productId);

            if (!success)
            {
                ConsoleHelper.DisplayError("Failed to delete product.");
                ConsoleHelper.PauseForUser();
                return;
            }

            ConsoleHelper.DisplaySuccess($"Product '{product.Name}' deleted successfully!");
            _persistenceService.SaveData();
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
            var products = _productService.GetAllProducts();
            if (!products.Any())
            {
                ConsoleHelper.DisplayWarning("No products available.");
                ConsoleHelper.PauseForUser();
                return;
            }

            ProductDisplayHelper.DisplayProductTable(products);

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

            var success = _productService.RestockProduct(productId, quantity);

            if (!success)
            {
                ConsoleHelper.DisplayError("Failed to restock product.");
                ConsoleHelper.PauseForUser();
                return;
            }

            var updatedProduct = _productService.GetProductById(productId);
            ConsoleHelper.DisplaySuccess("Product restocked successfully!");
            ConsoleHelper.DisplayInfo($"New stock level: {updatedProduct?.StockQuantity}");
            
            _persistenceService.SaveData();
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

        var lowStockProducts = _productService.GetLowStockProducts(10);

        if (!lowStockProducts.Any())
        {
            ConsoleHelper.DisplaySuccess("All products are adequately stocked!");
            ConsoleHelper.PauseForUser();
            return;
        }

        ConsoleHelper.DisplayWarning($"Found {lowStockProducts.Count} product(s) with low stock (<= 10 units):");
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

        DisplayOrdersGroupedByStatus(orders);
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

            DisplayOrdersTable(orders);

            var orderId = InputHelper.ReadInt("Enter Order ID to update (0 to cancel): ");
            if (orderId == 0) return;

            var selectedOrder = _orderService.GetOrderById(orderId);
            if (selectedOrder == null)
            {
                ConsoleHelper.DisplayError("Order not found.");
                ConsoleHelper.PauseForUser();
                return;
            }

            DisplayOrderSummary(selectedOrder);

            var newStatus = GetNewOrderStatus();

            var success = _orderService.UpdateOrderStatus(orderId, newStatus);

            if (!success)
            {
                ConsoleHelper.DisplayError("Failed to update order status.");
                ConsoleHelper.PauseForUser();
                return;
            }

            ConsoleHelper.DisplaySuccess($"Order #{orderId} status updated to {newStatus}!");
            
            if (newStatus == OrderStatus.Delivered)
            {
                ConsoleHelper.DisplayInfo("Delivery date has been set.");
            }
            
            _persistenceService.SaveData();
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
    /// Generate and display sales reports using Strategy Pattern
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

            // Strategy Pattern: Select the appropriate report strategy based on user choice
            IReportStrategy? strategy = null;
            var generator = new ReportGenerator();

            switch (choice)
            {
                case 1:
                    strategy = new SalesSummaryStrategy(_orderService);
                    break;
                case 2:
                    var limit = InputHelper.ReadPositiveInt("Enter number of top products to display: ");
                    strategy = new TopProductsStrategy(_orderService, limit);
                    break;
                case 3:
                    strategy = new SalesByCategoryStrategy(_orderService, _productService);
                    break;
                case 4:
                    return;
            }

            // Execute the selected strategy
            if (strategy != null)
            {
                generator.Generate(strategy);
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.DisplayError($"Error generating report: {ex.Message}");
        }

        ConsoleHelper.PauseForUser();
    }

    #endregion

    #region Helper Methods

    private void DisplayCurrentProductDetails(Product product)
    {
        Console.WriteLine($"\nCurrent details for: {product.Name}");
        Console.WriteLine($"Price: R{product.Price:F2}");
        Console.WriteLine($"Stock: {product.StockQuantity}");
        Console.WriteLine($"Category: {product.Category}");
        Console.WriteLine($"Description: {product.Description}");
        Console.WriteLine("\nEnter new values (press Enter to keep current):");
    }

    private (string Name, string Description, decimal Price, string Category) GetUpdatedProductDetails(Product product)
    {
        var newName = InputHelper.ReadString($"Name [{product.Name}]: ");
        if (string.IsNullOrWhiteSpace(newName)) 
            newName = product.Name;
        else if (newName.Length > 100)
        {
            ConsoleHelper.DisplayError("Product name cannot exceed 100 characters.");
            return (product.Name, product.Description, product.Price, product.Category);
        }
        else if (ValidationHelper.ContainsDangerousContent(newName))
        {
            ConsoleHelper.DisplayError("Product name contains invalid characters or patterns.");
            return (product.Name, product.Description, product.Price, product.Category);
        }

        var newDescription = InputHelper.ReadString($"Description [{product.Description}]: ");
        if (string.IsNullOrWhiteSpace(newDescription)) 
            newDescription = product.Description;
        else if (newDescription.Length > 500)
        {
            ConsoleHelper.DisplayError("Description cannot exceed 500 characters.");
            return (product.Name, product.Description, product.Price, product.Category);
        }
        else if (ValidationHelper.ContainsDangerousContent(newDescription))
        {
            ConsoleHelper.DisplayError("Description contains invalid characters or patterns.");
            return (product.Name, product.Description, product.Price, product.Category);
        }

        var priceInput = InputHelper.ReadString($"Price [R{product.Price:F2}]: ");
        decimal newPrice;
        
        if (string.IsNullOrWhiteSpace(priceInput))
        {
            newPrice = product.Price;
        }
        else
        {
            if (!decimal.TryParse(priceInput, out newPrice))
            {
                ConsoleHelper.DisplayError("Invalid price format. Price must be a valid number.");
                return (product.Name, product.Description, product.Price, product.Category);
            }
            
            if (newPrice <= 0)
            {
                ConsoleHelper.DisplayError("Price must be greater than zero.");
                return (product.Name, product.Description, product.Price, product.Category);
            }
            
            if (newPrice > 1000000)
            {
                ConsoleHelper.DisplayError("Price cannot exceed R1,000,000.");
                return (product.Name, product.Description, product.Price, product.Category);
            }
        }

        var categoryInput = InputHelper.ReadString($"Category [{product.Category}]: ");
        if (string.IsNullOrWhiteSpace(categoryInput)) 
            categoryInput = product.Category;
        else if (categoryInput.Length > 50)
        {
            ConsoleHelper.DisplayError("Category name cannot exceed 50 characters.");
            return (product.Name, product.Description, product.Price, product.Category);
        }
        else if (ValidationHelper.ContainsDangerousContent(categoryInput))
        {
            ConsoleHelper.DisplayError("Category name contains invalid characters or patterns.");
            return (product.Name, product.Description, product.Price, product.Category);
        }
        
        var newCategory = categoryInput;

        return (newName, newDescription, newPrice, newCategory);
    }

    private bool ConfirmDeletion(string productName)
    {
        Console.Write($"\nAre you sure you want to delete '{productName}'? (yes/no): ");
        var confirmation = Console.ReadLine()?.Trim().ToLower();
        return confirmation == "yes" || confirmation == "y";
    }

    private void DisplayOrdersGroupedByStatus(List<Order> orders)
    {
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
    }

    private void DisplayOrdersTable(List<Order> orders)
    {
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
    }

    private void DisplayOrderSummary(Order order)
    {
        Console.WriteLine($"\nOrder #{order.Id}");
        Console.WriteLine($"Current Status: {order.Status}");
        Console.WriteLine($"Customer ID: {order.CustomerId}");
        Console.WriteLine($"Total: R{order.TotalAmount:F2}");
    }

    private OrderStatus GetNewOrderStatus()
    {
        Console.WriteLine("\nSelect new status:");
        Console.WriteLine("1. Pending");
        Console.WriteLine("2. Processing");
        Console.WriteLine("3. Shipped");
        Console.WriteLine("4. Delivered");
        Console.WriteLine("5. Cancelled");

        var statusChoice = InputHelper.ReadMenuChoice(1, 5);

        return statusChoice switch
        {
            1 => OrderStatus.Pending,
            2 => OrderStatus.Processing,
            3 => OrderStatus.Shipped,
            4 => OrderStatus.Delivered,
            5 => OrderStatus.Cancelled,
            _ => OrderStatus.Pending
        };
    }

    #endregion
}
