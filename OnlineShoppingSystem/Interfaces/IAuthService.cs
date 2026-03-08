using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Interfaces;

/// <summary>
/// Interface for authentication and user management operations
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Register a new user in the system
    /// </summary>
    User? Register(string username, string email, string password, bool isAdmin);

    /// <summary>
    /// Login a user with username and password
    /// </summary>
    User? Login(string username, string password);

    /// <summary>
    /// Check if a username already exists
    /// </summary>
    bool UsernameExists(string username);

    /// <summary>
    /// Check if an email already exists
    /// </summary>
    bool EmailExists(string email);
}
