using OnlineShoppingSystem.Data;
using OnlineShoppingSystem.Factories;
using OnlineShoppingSystem.Helpers;
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

        if (username.Length > ValidationConstants.MaxUsernameLength)
        {
            throw new ArgumentException($"Username cannot exceed {ValidationConstants.MaxUsernameLength} characters.");
        }

        if (email.Length > ValidationConstants.MaxEmailLength)
        {
            throw new ArgumentException($"Email cannot exceed {ValidationConstants.MaxEmailLength} characters.");
        }

        if (password.Length < 6)
        {
            throw new ArgumentException("Password must be at least 6 characters long.");
        }

        if (password.Length > 100)
        {
            throw new ArgumentException("Password cannot exceed 100 characters.");
        }

        if (!ValidationHelper.IsValidEmail(email))
        {
            throw new ArgumentException("Invalid email format.");
        }

        if (ValidationHelper.ContainsDangerousContent(username))
        {
            throw new ArgumentException("Username contains invalid characters or patterns.");
        }

        if (UsernameExists(username))
        {
            throw new InvalidOperationException("Username already exists.");
        }

        if (EmailExists(email))
        {
            throw new InvalidOperationException("Email already exists.");
        }

        // Use Factory Pattern to create the appropriate user type
        var role = isAdmin ? UserRole.Administrator : UserRole.Customer;
        var newUser = UserFactory.CreateUser(username, email, password, role);

        // Create a cart for new customers
        if (newUser is Customer customer)
        {
            var cart = new Cart
            {
                Id = _dataStore.GetNextCartId(),
                CustomerId = customer.Id
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
