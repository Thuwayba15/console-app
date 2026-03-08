namespace OnlineShoppingSystem.Helpers;

/// <summary>
/// Helper class for common validation operations
/// </summary>
public static class ValidationHelper
{
    /// <summary>
    /// Validate that a string is not null or whitespace
    /// </summary>
    public static bool IsNullOrEmpty(string? value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    /// <summary>
    /// Validate email format (simple validation)
    /// </summary>
    public static bool IsValidEmail(string email)
    {
        if (IsNullOrEmpty(email))
        {
            return false;
        }

        return email.Contains("@") && email.Contains(".");
    }

    /// <summary>
    /// Validate that a value is positive
    /// </summary>
    public static bool IsPositive(decimal value)
    {
        return value > 0;
    }

    /// <summary>
    /// Validate that a value is positive
    /// </summary>
    public static bool IsPositive(int value)
    {
        return value > 0;
    }

    /// <summary>
    /// Validate that a rating is within the valid range (1-5)
    /// </summary>
    public static bool IsValidRating(int rating)
    {
        return rating >= 1 && rating <= 5;
    }
}
