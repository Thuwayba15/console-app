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
            { 1, new AddProductCommand(productService, persistenceService) },
            { 2, new UpdateProductCommand(productService, persistenceService) },
            { 3, new DeleteProductCommand(productService, persistenceService) },
            { 4, new RestockProductCommand(productService, persistenceService) },
            { 5, new ViewProductsCommand(productService) },
            { 6, new ViewOrdersCommand(orderService) },
            { 7, new UpdateOrderStatusCommand(orderService, persistenceService) },
            { 8, new ViewLowStockCommand(productService) },
            { 9, new GenerateReportsCommand(orderService, productService) }
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

            if (choice == 10)
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
