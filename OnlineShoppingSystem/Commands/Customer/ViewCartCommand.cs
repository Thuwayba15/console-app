using OnlineShoppingSystem.Helpers;
using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Commands.Customer;

/// <summary>
/// Command for viewing the shopping cart
/// </summary>
public class ViewCartCommand : ICommand
{
    private readonly Models.Customer _customer;
    private readonly ICartService _cartService;

    public ViewCartCommand(Models.Customer customer, ICartService cartService)
    {
        _customer = customer;
        _cartService = cartService;
    }

    public void Execute()
    {
        ConsoleHelper.DisplayHeader("YOUR SHOPPING CART");

        var cart = _cartService.GetCart(_customer.Id);

        if (cart == null || !cart.Items.Any())
        {
            ConsoleHelper.DisplayWarning("Your cart is empty.");
            ConsoleHelper.PauseForUser();
            return;
        }

        OrderDisplayHelper.DisplayCart(cart);
        ConsoleHelper.PauseForUser();
    }

    public string GetName() => "View Cart";
}
