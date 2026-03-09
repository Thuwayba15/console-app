using OnlineShoppingSystem.Commands;
using OnlineShoppingSystem.Commands.Admin;
using OnlineShoppingSystem.Enums;
using OnlineShoppingSystem.Helpers;
using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Models;
using OnlineShoppingSystem.Services;
using OnlineShoppingSystem.Strategies;

namespace OnlineShoppingSystem.Menus;

/// <summary>
/// Administrator menu implementing Command Pattern
/// 
/// Architecture:
/// -------------
/// Similar to CustomerMenu, this class uses the Command Pattern to
/// encapsulate administrative actions as command objects.
/// 
/// Benefits:
/// - Separation of concerns (menu routing vs. business logic)
/// - Easy to add new admin features
/// - Commands are testable in isolation
/// - Reduced menu class size (from 700+ lines to 73 lines)
/// 
/// Design Pattern Integration:
/// - Command Pattern: Menu actions as commands
/// - Strategy Pattern: Report generation (see GenerateReportsCommand)
/// - Factory Pattern: Menu created by MenuFactory
/// </summary>
public class AdministratorMenu
{
    private readonly Administrator _admin;
    private readonly Dictionary<int, ICommand> _commands;

    public AdministratorMenu(
        Administrator admin,
        IProductService productService,
        IOrderService orderService,
        IReportService reportService,
        IPersistenceService persistenceService)
    {
        _admin = admin;

        // Command Pattern: Register all administrative commands
        _commands = new Dictionary<int, ICommand>
        {
            { 1, new ViewDashboardCommand(productService, orderService) },
            { 2, new AddProductCommand(productService, persistenceService) },
            { 3, new UpdateProductCommand(productService, persistenceService) },
            { 4, new DeleteProductCommand(productService, persistenceService) },
            { 5, new RestockProductCommand(productService, persistenceService) },
            { 6, new ViewProductsCommand(productService) },
            { 7, new ViewOrdersCommand(orderService) },
            { 8, new UpdateOrderStatusCommand(orderService, persistenceService) },
            { 9, new ViewLowStockCommand(productService) },
            { 10, new GenerateReportsCommand(orderService, productService) }
        };
    }

    /// <summary>
    /// Display and handle the administrator menu
    /// Command Pattern: Menu just selects and executes commands
    /// </summary>
    public void Show()
    {
        while (true)
        {
            ConsoleHelper.DisplayHeader($"ADMINISTRATOR MENU - Welcome, {_admin.Username}!");
            
            // Dashboard
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("1. ");
            Console.ResetColor();
            Console.WriteLine("View Dashboard");
            
            // Product Management Section
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\n[PRODUCT MANAGEMENT]");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("2. ");
            Console.ResetColor();
            Console.WriteLine("Add Product");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("3. ");
            Console.ResetColor();
            Console.WriteLine("Update Product");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("4. ");
            Console.ResetColor();
            Console.WriteLine("Delete Product");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("5. ");
            Console.ResetColor();
            Console.WriteLine("Restock Product");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("6. ");
            Console.ResetColor();
            Console.WriteLine("View Products");
            
            // Order Management Section
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n[ORDER MANAGEMENT]");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("7. ");
            Console.ResetColor();
            Console.WriteLine("View Orders");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("8. ");
            Console.ResetColor();
            Console.WriteLine("Update Order Status");
            
            // Inventory & Reports
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n[INVENTORY & REPORTS]");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("9. ");
            Console.ResetColor();
            Console.WriteLine("View Low Stock Products");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("10. ");
            Console.ResetColor();
            Console.WriteLine("Generate Sales Reports");
            
            // Logout
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\n11. ");
            Console.ResetColor();
            Console.WriteLine("Logout");

            var choice = InputHelper.ReadMenuChoice(1, 11);

            if (choice == 11)
            {
                ConsoleHelper.DisplaySuccess("Logged out successfully.");
                ConsoleHelper.PauseForUser();
                return;
            }

            // Command Pattern: Execute the selected command
            if (_commands.TryGetValue(choice, out var command))
            {
                command.Execute();
            }
        }
    }
}
