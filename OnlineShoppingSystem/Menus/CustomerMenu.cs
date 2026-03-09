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
            Console.WriteLine("1. Browse Products");
            Console.WriteLine("2. Search Products");
            Console.WriteLine("3. Add Product to Cart");
            Console.WriteLine("4. View Cart");
            Console.WriteLine("5. Update Cart");
            Console.WriteLine("6. Checkout");
            Console.WriteLine("7. View Wallet Balance");
            Console.WriteLine("8. Add Wallet Funds");
            Console.WriteLine("9. View Order History");
            Console.WriteLine("10. Track Orders");
            Console.WriteLine("11. Review Products");
            Console.WriteLine("12. Logout");

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
