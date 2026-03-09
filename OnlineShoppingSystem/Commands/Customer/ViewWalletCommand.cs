using OnlineShoppingSystem.Helpers;
using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Commands.Customer;

/// <summary>
/// Command for viewing wallet balance
/// </summary>
public class ViewWalletCommand : ICommand
{
    private readonly Models.Customer _customer;
    private readonly IPaymentService _paymentService;

    public ViewWalletCommand(Models.Customer customer, IPaymentService paymentService)
    {
        _customer = customer;
        _paymentService = paymentService;
    }

    public void Execute()
    {
        ConsoleHelper.DisplayHeader("WALLET BALANCE");
        var balance = _paymentService.GetWalletBalance(_customer.Id);
        _customer.WalletBalance = balance;
        Console.WriteLine($"\nCurrent Balance: R{balance:F2}");
        ConsoleHelper.PauseForUser();
    }

    public string GetName() => "View Wallet Balance";
}
