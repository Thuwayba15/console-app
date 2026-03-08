using OnlineShoppingSystem.Enums;

namespace OnlineShoppingSystem.Models;

/// <summary>
/// Represents a customer who can browse and purchase products
/// </summary>
public class Customer : User
{
    public decimal WalletBalance { get; set; }
    public List<int> OrderIds { get; set; }

    public Customer()
    {
        Role = UserRole.Customer;
        WalletBalance = 0;
        OrderIds = new List<int>();
    }
}
