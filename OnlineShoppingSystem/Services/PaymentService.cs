using OnlineShoppingSystem.Data;
using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Services;

/// <summary>
/// Service for handling payments and wallet operations
/// </summary>
public class PaymentService : IPaymentService
{
    private readonly AppDataStore _dataStore;

    public PaymentService()
    {
        _dataStore = AppDataStore.Instance;
    }

    public bool ProcessPayment(int customerId, int orderId, decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Payment amount must be greater than zero.");
        }

        var customer = _dataStore.Users.OfType<Customer>().FirstOrDefault(c => c.Id == customerId);
        if (customer == null)
        {
            return false;
        }

        if (customer.WalletBalance < amount)
        {
            return false;
        }

        customer.WalletBalance -= amount;

        var payment = new Payment
        {
            Id = _dataStore.GetNextPaymentId(),
            OrderId = orderId,
            CustomerId = customerId,
            Amount = amount,
            IsSuccessful = true
        };

        _dataStore.Payments.Add(payment);
        return true;
    }

    public bool AddFunds(int customerId, decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than zero.");
        }

        var customer = _dataStore.Users.OfType<Customer>().FirstOrDefault(c => c.Id == customerId);
        if (customer == null)
        {
            return false;
        }

        customer.WalletBalance += amount;
        return true;
    }

    public decimal GetWalletBalance(int customerId)
    {
        var customer = _dataStore.Users.OfType<Customer>().FirstOrDefault(c => c.Id == customerId);
        return customer?.WalletBalance ?? 0;
    }
}
