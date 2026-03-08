using OnlineShoppingSystem.Enums;

namespace OnlineShoppingSystem.Models;

/// <summary>
/// Represents an administrator who manages products and orders
/// </summary>
public class Administrator : User
{
    public Administrator()
    {
        Role = UserRole.Administrator;
    }
}
