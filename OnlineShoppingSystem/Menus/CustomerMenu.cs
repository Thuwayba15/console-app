using OnlineShoppingSystem.Helpers;
using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Menus;

/// <summary>
/// Customer menu for browsing, shopping, and managing orders
/// </summary>
public class CustomerMenu
{
    private readonly Customer _customer;
    private readonly IProductService _productService;
    private readonly ICartService _cartService;
    private readonly IOrderService _orderService;
    private readonly IPaymentService _paymentService;
    private readonly IReviewService _reviewService;

    public CustomerMenu(
        Customer customer,
        IProductService productService,
        ICartService cartService,
        IOrderService orderService,
        IPaymentService paymentService,
        IReviewService reviewService)
    {
        _customer = customer;
        _productService = productService;
        _cartService = cartService;
        _orderService = orderService;
        _paymentService = paymentService;
        _reviewService = reviewService;
    }

    /// <summary>
    /// Display and handle the customer menu
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

            switch (choice)
            {
                case 1:
                    ConsoleHelper.DisplayInfo("Browse Products feature coming in next milestone.");
                    ConsoleHelper.PauseForUser();
                    break;
                case 2:
                    ConsoleHelper.DisplayInfo("Search Products feature coming in next milestone.");
                    ConsoleHelper.PauseForUser();
                    break;
                case 3:
                    ConsoleHelper.DisplayInfo("Add to Cart feature coming in next milestone.");
                    ConsoleHelper.PauseForUser();
                    break;
                case 4:
                    ConsoleHelper.DisplayInfo("View Cart feature coming in next milestone.");
                    ConsoleHelper.PauseForUser();
                    break;
                case 5:
                    ConsoleHelper.DisplayInfo("Update Cart feature coming in next milestone.");
                    ConsoleHelper.PauseForUser();
                    break;
                case 6:
                    ConsoleHelper.DisplayInfo("Checkout feature coming in next milestone.");
                    ConsoleHelper.PauseForUser();
                    break;
                case 7:
                    ConsoleHelper.DisplayInfo($"Current Wallet Balance: ${_customer.WalletBalance:F2}");
                    ConsoleHelper.PauseForUser();
                    break;
                case 8:
                    ConsoleHelper.DisplayInfo("Add Funds feature coming in next milestone.");
                    ConsoleHelper.PauseForUser();
                    break;
                case 9:
                    ConsoleHelper.DisplayInfo("Order History feature coming in next milestone.");
                    ConsoleHelper.PauseForUser();
                    break;
                case 10:
                    ConsoleHelper.DisplayInfo("Track Orders feature coming in next milestone.");
                    ConsoleHelper.PauseForUser();
                    break;
                case 11:
                    ConsoleHelper.DisplayInfo("Review Products feature coming in next milestone.");
                    ConsoleHelper.PauseForUser();
                    break;
                case 12:
                    ConsoleHelper.DisplaySuccess("Logged out successfully.");
                    ConsoleHelper.PauseForUser();
                    return;
            }
        }
    }
}
