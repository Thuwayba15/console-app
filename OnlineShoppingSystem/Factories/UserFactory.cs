using OnlineShoppingSystem.Data;
using OnlineShoppingSystem.Enums;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Factories;

/// <summary>
/// Factory for creating user objects based on role
/// Implements the Factory Pattern to encapsulate user creation logic
/// </summary>
public static class UserFactory
{
    /// <summary>
    /// Create a user of the appropriate type based on the specified role
    /// </summary>
    /// <param name="username">Username for the new user</param>
    /// <param name="email">Email address for the new user</param>
    /// <param name="password">Password for the new user</param>
    /// <param name="role">The role (Customer or Administrator)</param>
    /// <returns>A User object of the correct type (Customer or Administrator)</returns>
    public static User CreateUser(string username, string email, string password, UserRole role)
    {
        var dataStore = AppDataStore.Instance;
        var userId = dataStore.GetNextUserId();

        // Factory Pattern: Create the appropriate user type based on role
        return role switch
        {
            UserRole.Administrator => new Administrator
            {
                Id = userId,
                Username = username.Trim(),
                Email = email.Trim().ToLower(),
                Password = password,
                CreatedAt = DateTime.Now
            },
            
            UserRole.Customer => new Customer
            {
                Id = userId,
                Username = username.Trim(),
                Email = email.Trim().ToLower(),
                Password = password,
                WalletBalance = 0,
                OrderIds = new List<int>(),
                CreatedAt = DateTime.Now
            },
            
            _ => throw new ArgumentException($"Unknown user role: {role}")
        };
    }

    /// <summary>
    /// Create a customer with an initial wallet balance
    /// </summary>
    public static Customer CreateCustomerWithBalance(string username, string email, string password, decimal initialBalance)
    {
        var customer = (Customer)CreateUser(username, email, password, UserRole.Customer);
        customer.WalletBalance = initialBalance;
        return customer;
    }
}
