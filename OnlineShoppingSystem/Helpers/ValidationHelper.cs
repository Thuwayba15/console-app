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
        return rating >= ValidationConstants.MinRating && rating <= ValidationConstants.MaxRating;
    }

    /// <summary>
    /// Validate string length
    /// </summary>
    public static bool IsValidLength(string value, int maxLength)
    {
        return !string.IsNullOrEmpty(value) && value.Length <= maxLength;
    }

    /// <summary>
    /// Sanitize string input by removing potentially dangerous characters
    /// </summary>
    public static string SanitizeInput(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        // Remove HTML/script tags patterns
        var sanitized = input
            .Replace("<", "")
            .Replace(">", "")
            .Replace("script", "")
            .Replace("javascript:", "")
            .Replace("onerror", "")
            .Replace("onclick", "");

        // Remove SQL injection patterns
        sanitized = sanitized
            .Replace("--", "")
            .Replace("';", "'")
            .Replace("\";", "\"")
            .Replace("DROP", "")
            .Replace("DELETE", "")
            .Replace("UPDATE", "")
            .Replace("INSERT", "");

        return sanitized.Trim();
    }

    /// <summary>
    /// Check if string contains potentially dangerous content
    /// </summary>
    public static bool ContainsDangerousContent(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        var dangerous = new[]
        {
            "<script", "</script", "javascript:", "onerror=", "onclick=",
            "DROP TABLE", "DELETE FROM", "';--", "\";--", "1=1", "OR 1=1"
        };

        var lowerInput = input.ToLower();
        return dangerous.Any(pattern => lowerInput.Contains(pattern.ToLower()));
    }
}
