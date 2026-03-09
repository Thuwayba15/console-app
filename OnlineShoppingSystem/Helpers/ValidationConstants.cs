namespace OnlineShoppingSystem.Helpers;

/// <summary>
/// Constants for input validation limits
/// </summary>
public static class ValidationConstants
{
    // String length limits
    public const int MaxProductNameLength = 100;
    public const int MaxDescriptionLength = 500;
    public const int MaxCategoryLength = 50;
    public const int MaxUsernameLength = 50;
    public const int MaxEmailLength = 100;
    public const int MaxCommentLength = 1000;
    public const int MinPasswordLength = 6;
    public const int MaxPasswordLength = 100;

    // Numeric limits
    public const decimal MaxPrice = 1000000m;
    public const int MaxStockQuantity = 1000000;
    public const decimal MaxWalletAmount = 10000000m;
    public const int MaxQuantity = 10000;

    // Rating limits
    public const int MinRating = 1;
    public const int MaxRating = 5;

    // Messages
    public const string ProductNameTooLong = "Product name cannot exceed 100 characters.";
    public const string DescriptionTooLong = "Description cannot exceed 500 characters.";
    public const string CategoryTooLong = "Category name cannot exceed 50 characters.";
    public const string PriceTooHigh = "Price cannot exceed R1,000,000.";
    public const string StockTooHigh = "Stock quantity cannot exceed 1,000,000 units.";
    public const string InvalidPrice = "Price must be greater than zero.";
    public const string InvalidStock = "Stock quantity cannot be negative.";
    public const string PasswordTooShort = "Password must be at least 6 characters long.";
    public const string PasswordTooLong = "Password cannot exceed 100 characters.";
}
