using OnlineShoppingSystem.Helpers;
using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Commands.Customer;

/// <summary>
/// Command for adding funds to wallet
/// </summary>
public class AddFundsCommand : ICommand
{
    private readonly Models.Customer _customer;
    private readonly IPaymentService _paymentService;
    private readonly IPersistenceService _persistenceService;

    public AddFundsCommand(Models.Customer customer, IPaymentService paymentService, IPersistenceService persistenceService)
    {
        _customer = customer;
        _paymentService = paymentService;
        _persistenceService = persistenceService;
    }

    public void Execute()
    {
        ConsoleHelper.DisplayHeader("ADD WALLET FUNDS");

        var currentBalance = _paymentService.GetWalletBalance(_customer.Id);
        Console.WriteLine($"Current Balance: R{currentBalance:F2}\n");

        try
        {
            var amount = InputHelper.ReadPositiveDecimal("Enter amount to add: R");
            var success = _paymentService.AddFunds(_customer.Id, amount);

            if (success)
            {
                var newBalance = _paymentService.GetWalletBalance(_customer.Id);
                _customer.WalletBalance = newBalance;
                _persistenceService.SaveData();

                ConsoleHelper.DisplaySuccess($"Added R{amount:F2} to your wallet.");
                ConsoleHelper.DisplayInfo($"New Balance: R{newBalance:F2}");
            }
            else
            {
                ConsoleHelper.DisplayError("Failed to add funds.");
            }
        }
        catch (ArgumentException ex)
        {
            ConsoleHelper.DisplayError(ex.Message);
        }
        catch (Exception ex)
        {
            ConsoleHelper.DisplayError($"Error adding funds: {ex.Message}");
        }

        ConsoleHelper.PauseForUser();
    }

    public string GetName() => "Add Wallet Funds";
}
