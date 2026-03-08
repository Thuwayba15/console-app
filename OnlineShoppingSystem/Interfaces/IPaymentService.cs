using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Interfaces;

/// <summary>
/// Interface for payment operations
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// Process a payment for an order using wallet balance
    /// </summary>
    bool ProcessPayment(int customerId, int orderId, decimal amount);

    /// <summary>
    /// Add funds to a customer's wallet
    /// </summary>
    bool AddFunds(int customerId, decimal amount);

    /// <summary>
    /// Get the wallet balance for a customer
    /// </summary>
    decimal GetWalletBalance(int customerId);
}
