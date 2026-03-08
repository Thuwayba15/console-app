using OnlineShoppingSystem.Data;
using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Models;
using OnlineShoppingSystem.Enums;

namespace OnlineShoppingSystem.Services;

/// <summary>
/// Service for handling user authentication and registration
/// </summary>
public class AuthService : IAuthService
{
    private readonly AppDataStore _dataStore;

    public AuthService()
    {
        _dataStore = AppDataStore.Instance;
    }

    public User? Register(string username, string email, string password, bool isAdmin)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Username, email, and password are required.");
        }

        if (UsernameExists(username))
        {
            throw new InvalidOperationException("Username already exists.");
        }

        if (EmailExists(email))
        {
            throw new InvalidOperationException("Email already exists.");
        }

        User newUser;
        if (isAdmin)
        {
            newUser = new Administrator
            {
                Id = _dataStore.GetNextUserId(),
                Username = username,
                Email = email,
                Password = password
            };
        }
        else
        {
            newUser = new Customer
            {
                Id = _dataStore.GetNextUserId(),
                Username = username,
                Email = email,
                Password = password,
                WalletBalance = 0
            };

            // Create a cart for the new customer
            var cart = new Cart
            {
                Id = _dataStore.GetNextCartId(),
                CustomerId = newUser.Id
            };
            _dataStore.Carts.Add(cart);
        }

        _dataStore.Users.Add(newUser);
        return newUser;
    }

    public User? Login(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Username and password are required.");
        }

        var user = _dataStore.Users.FirstOrDefault(u => 
            u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && 
            u.Password == password);

        return user;
    }

    public bool UsernameExists(string username)
    {
        return _dataStore.Users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
    }

    public bool EmailExists(string email)
    {
        return _dataStore.Users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }
}
