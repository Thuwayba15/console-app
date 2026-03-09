using OnlineShoppingSystem.Helpers;
using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Commands.Customer;

/// <summary>
/// Command for processing checkout and creating an order
/// </summary>
public class CheckoutCommand : ICommand
{
    private readonly Models.Customer _customer;
    private readonly ICartService _cartService;
    private readonly IOrderService _orderService;
    private readonly IPaymentService _paymentService;
    private readonly IPersistenceService _persistenceService;

    public CheckoutCommand(
        Models.Customer customer,
        ICartService cartService,
        IOrderService orderService,
        IPaymentService paymentService,
        IPersistenceService persistenceService)
    {
        _customer = customer;
        _cartService = cartService;
        _orderService = orderService;
        _paymentService = paymentService;
        _persistenceService = persistenceService;
    }

    public void Execute()
    {
        ConsoleHelper.DisplayHeader("CHECKOUT");

        var cart = _cartService.GetCart(_customer.Id);

        if (cart == null || !cart.Items.Any())
        {
            ConsoleHelper.DisplayError("Your cart is empty. Add products before checking out.");
            ConsoleHelper.PauseForUser();
            return;
        }

        OrderDisplayHelper.DisplayCheckoutSummary(cart);

        var currentBalance = _paymentService.GetWalletBalance(_customer.Id);
        Console.WriteLine($"\nYour Wallet Balance: R{currentBalance:F2}");

        if (currentBalance < cart.GetTotal())
        {
            ConsoleHelper.DisplayError($"Insufficient funds. You need R{cart.GetTotal() - currentBalance:F2} more.");
            ConsoleHelper.DisplayInfo("Please add funds to your wallet (option 8) and try again.");
            ConsoleHelper.PauseForUser();
            return;
        }

        Console.Write("\nConfirm checkout? (yes/no): ");
        var confirmation = Console.ReadLine()?.Trim().ToLower();

        if (confirmation != "yes" && confirmation != "y")
        {
            ConsoleHelper.DisplayInfo("Checkout cancelled.");
            ConsoleHelper.PauseForUser();
            return;
        }

        try
        {
            var order = _orderService.CreateOrder(_customer.Id);

            if (order != null)
            {
                ConsoleHelper.DisplaySuccess($"Order #{order.Id} created successfully!");
                ConsoleHelper.DisplayInfo($"Total: R{order.TotalAmount:F2}");
                ConsoleHelper.DisplayInfo($"New Wallet Balance: R{_paymentService.GetWalletBalance(_customer.Id):F2}");
                ConsoleHelper.DisplayInfo($"Order Status: {order.Status}");

                _customer.WalletBalance = _paymentService.GetWalletBalance(_customer.Id);
                _persistenceService.SaveData();
            }
            else
            {
                ConsoleHelper.DisplayError("Failed to create order.");
            }
        }
        catch (InvalidOperationException ex)
        {
            ConsoleHelper.DisplayError(ex.Message);
        }
        catch (Exception ex)
        {
            ConsoleHelper.DisplayError($"Checkout error: {ex.Message}");
        }

        ConsoleHelper.PauseForUser();
    }

    public string GetName() => "Checkout";
}
