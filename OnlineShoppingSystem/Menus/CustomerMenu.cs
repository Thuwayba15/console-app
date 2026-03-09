using OnlineShoppingSystem.Commands;
using OnlineShoppingSystem.Commands.Customer;
using OnlineShoppingSystem.Helpers;
using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Menus;

/// <summary>
/// Customer menu implementing Command Pattern
/// 
/// Architecture:
/// -------------
/// This menu demonstrates the Command Pattern where each menu action is
/// encapsulated as an ICommand object. This provides several benefits:
/// 
/// 1. Single Responsibility - Each command class handles one specific action
/// 2. Open/Closed Principle - Add new features by creating new command classes
/// 3. Testability - Commands can be tested independently
/// 4. Maintainability - Menu class stays small and focused on routing
/// 
/// The menu acts as an "invoker" that stores commands in a dictionary
/// and executes them based on user choice. Commands are "receivers" that
/// contain the actual business logic.
/// 
/// Adding a new menu option:
/// 1. Create a new command class implementing ICommand
/// 2. Register it in the _commands dictionary
/// 3. Add the menu option to Show() method
/// 
/// No modifications to existing commands or business logic required!
/// </summary>
public class CustomerMenu
{
    private readonly Customer _customer;
    private readonly Dictionary<int, ICommand> _commands;

    public CustomerMenu(
        Customer customer,
        IProductService productService,
        ICartService cartService,
        IOrderService orderService,
        IPaymentService paymentService,
        IReviewService reviewService,
        IPersistenceService persistenceService)
    {
        _customer = customer;

        // Command Pattern: Register all menu commands
        // Each command is independent and self-contained
        _commands = new Dictionary<int, ICommand>
        {
            { 1, new BrowseProductsCommand(productService) },
            { 2, new SearchProductsCommand(productService) },
            { 3, new AddToCartCommand(customer, cartService, productService) },
            { 4, new ViewCartCommand(customer, cartService) },
            { 5, new UpdateCartCommand(customer, cartService) },
            { 6, new CheckoutCommand(customer, cartService, orderService, paymentService, persistenceService) },
            { 7, new ViewWalletCommand(customer, paymentService) },
            { 8, new AddFundsCommand(customer, paymentService, persistenceService) },
            { 9, new ViewOrderHistoryCommand(customer, orderService) },
            { 10, new TrackOrdersCommand(customer, orderService) },
            { 11, new ReviewProductsCommand(customer, productService, orderService, reviewService) }
        };
    }

    /// <summary>
    /// Display and handle the customer menu
    /// Command Pattern: Menu just selects and executes commands
    /// </summary>
    public void Show()
    {
        while (true)
        {
            ConsoleHelper.DisplayHeader($"CUSTOMER MENU - Welcome, {_customer.Username}!");
            
            // Shopping Section
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n[SHOPPING]");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("1. ");
            Console.ResetColor();
            Console.WriteLine("Browse Products");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("2. ");
            Console.ResetColor();
            Console.WriteLine("Search Products");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("3. ");
            Console.ResetColor();
            Console.WriteLine("Add Product to Cart");
            
            // Cart Section
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n[CART]");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("4. ");
            Console.ResetColor();
            Console.WriteLine("View Cart");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("5. ");
            Console.ResetColor();
            Console.WriteLine("Update Cart");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("6. ");
            Console.ResetColor();
            Console.WriteLine("Checkout");
            
            // Wallet Section
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n[WALLET]");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("7. ");
            Console.ResetColor();
            Console.WriteLine("View Wallet Balance");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("8. ");
            Console.ResetColor();
            Console.WriteLine("Add Wallet Funds");
            
            // Orders Section
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\n[ORDERS]");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("9. ");
            Console.ResetColor();
            Console.WriteLine("View Order History");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("10. ");
            Console.ResetColor();
            Console.WriteLine("Track Orders");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("11. ");
            Console.ResetColor();
            Console.WriteLine("Review Products");
            
            // Logout
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\n12. ");
            Console.ResetColor();
            Console.WriteLine("Logout");

            var choice = InputHelper.ReadMenuChoice(1, 12);

            if (choice == 12)
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
