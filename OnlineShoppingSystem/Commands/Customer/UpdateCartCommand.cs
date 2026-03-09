using OnlineShoppingSystem.Helpers;
using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Commands.Customer;

/// <summary>
/// Command for updating cart item quantities or removing items
/// </summary>
public class UpdateCartCommand : ICommand
{
    private readonly Models.Customer _customer;
    private readonly ICartService _cartService;

    public UpdateCartCommand(Models.Customer customer, ICartService cartService)
    {
        _customer = customer;
        _cartService = cartService;
    }

    public void Execute()
    {
        ConsoleHelper.DisplayHeader("UPDATE CART");

        var cart = _cartService.GetCart(_customer.Id);

        if (cart == null || !cart.Items.Any())
        {
            ConsoleHelper.DisplayWarning("Your cart is empty.");
            ConsoleHelper.PauseForUser();
            return;
        }

        OrderDisplayHelper.DisplayCart(cart);

        try
        {
            var productId = InputHelper.ReadInt("Enter Product ID to update (0 to cancel): ");

            if (productId == 0) return;

            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (cartItem == null)
            {
                ConsoleHelper.DisplayError("Product not found in cart.");
                ConsoleHelper.PauseForUser();
                return;
            }

            Console.WriteLine($"\nCurrent quantity: {cartItem.Quantity}");
            Console.WriteLine("Enter new quantity (0 to remove item):");
            var newQuantity = InputHelper.ReadInt("New quantity: ");

            if (newQuantity < 0)
            {
                ConsoleHelper.DisplayError("Quantity cannot be negative.");
                ConsoleHelper.PauseForUser();
                return;
            }

            if (newQuantity == 0)
            {
                var removed = _cartService.RemoveFromCart(_customer.Id, productId);
                if (removed)
                {
                    ConsoleHelper.DisplaySuccess($"Removed {cartItem.ProductName} from cart.");
                }
            }
            else
            {
                var updated = _cartService.UpdateCartItem(_customer.Id, productId, newQuantity);
                if (updated)
                {
                    ConsoleHelper.DisplaySuccess($"Updated {cartItem.ProductName} quantity to {newQuantity}.");
                }
            }
        }
        catch (InvalidOperationException ex)
        {
            ConsoleHelper.DisplayError(ex.Message);
        }
        catch (Exception ex)
        {
            ConsoleHelper.DisplayError($"Error updating cart: {ex.Message}");
        }

        ConsoleHelper.PauseForUser();
    }

    public string GetName() => "Update Cart";
}
