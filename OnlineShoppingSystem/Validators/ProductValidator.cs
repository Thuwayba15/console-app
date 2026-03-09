using OnlineShoppingSystem.Helpers;

namespace OnlineShoppingSystem.Validators;

/// <summary>
/// Centralized product validation to eliminate code duplication
/// Provides consistent validation rules across all product operations
/// </summary>
public static class ProductValidator
{
    /// <summary>
    /// Validate product name
    /// </summary>
    public static (bool IsValid, string ErrorMessage) ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return (false, "Product name is required.");

        if (name.Length > ValidationConstants.MaxProductNameLength)
            return (false, ValidationConstants.ProductNameTooLong);

        if (ValidationHelper.ContainsDangerousContent(name))
            return (false, "Product name contains invalid characters or patterns.");

        return (true, string.Empty);
    }

    /// <summary>
    /// Validate product description
    /// </summary>
    public static (bool IsValid, string ErrorMessage) ValidateDescription(string description)
    {
        // Description can be empty (unlike name), but if provided, must be valid
        if (string.IsNullOrWhiteSpace(description))
            return (true, string.Empty); // Description is optional

        if (description.Length > ValidationConstants.MaxDescriptionLength)
            return (false, ValidationConstants.DescriptionTooLong);

        if (ValidationHelper.ContainsDangerousContent(description))
            return (false, "Description contains invalid characters or patterns.");

        return (true, string.Empty);
    }

    /// <summary>
    /// Validate product price
    /// </summary>
    public static (bool IsValid, string ErrorMessage) ValidatePrice(decimal price)
    {
        if (price <= 0)
            return (false, "Price must be greater than zero.");

        if (price > ValidationConstants.MaxPrice)
            return (false, ValidationConstants.PriceTooHigh);

        return (true, string.Empty);
    }

    /// <summary>
    /// Validate product category
    /// </summary>
    public static (bool IsValid, string ErrorMessage) ValidateCategory(string category)
    {
        // Category can be empty (optional)
        if (string.IsNullOrWhiteSpace(category))
            return (true, string.Empty); // Category is optional

        if (category.Length > ValidationConstants.MaxCategoryLength)
            return (false, ValidationConstants.CategoryTooLong);

        if (ValidationHelper.ContainsDangerousContent(category))
            return (false, "Category contains invalid characters or patterns.");

        return (true, string.Empty);
    }

    /// <summary>
    /// Validate stock quantity
    /// </summary>
    public static (bool IsValid, string ErrorMessage) ValidateStockQuantity(int quantity)
    {
        if (quantity < 0)
            return (false, "Stock quantity cannot be negative.");

        if (quantity > ValidationConstants.MaxStockQuantity)
            return (false, ValidationConstants.StockTooHigh);

        return (true, string.Empty);
    }

    /// <summary>
    /// Validate all product fields at once
    /// </summary>
    public static (bool IsValid, string ErrorMessage) ValidateProduct(string name, string description, decimal price, string category, int stockQuantity)
    {
        var nameValidation = ValidateName(name);
        if (!nameValidation.IsValid) return nameValidation;

        var descValidation = ValidateDescription(description);
        if (!descValidation.IsValid) return descValidation;

        var priceValidation = ValidatePrice(price);
        if (!priceValidation.IsValid) return priceValidation;

        var categoryValidation = ValidateCategory(category);
        if (!categoryValidation.IsValid) return categoryValidation;

        var stockValidation = ValidateStockQuantity(stockQuantity);
        if (!stockValidation.IsValid) return stockValidation;

        return (true, string.Empty);
    }
}
