using OnlineShoppingSystem.Enums;

namespace OnlineShoppingSystem.Models;

/// <summary>
/// Base class for all users in the system
/// </summary>
public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; }

    public User()
    {
        Username = string.Empty;
        Email = string.Empty;
        Password = string.Empty;
        CreatedAt = DateTime.Now;
    }
}
