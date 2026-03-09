using OnlineShoppingSystem.Validators;
using Xunit;

namespace OnlineShoppingSystem.Tests.Validators;

/// <summary>
/// Unit tests for ProductValidator
/// </summary>
public class ProductValidatorTests
{
    #region Name Validation Tests

    [Fact]
    public void ValidateName_ValidName_ReturnsTrue()
    {
        // Arrange
        var validName = "Valid Product Name";

        // Act
        var result = ProductValidator.ValidateName(validName);

        // Assert
        Assert.True(result.IsValid);
        Assert.Equal(string.Empty, result.ErrorMessage);
    }

    [Fact]
    public void ValidateName_EmptyName_ReturnsFalse()
    {
        // Arrange
        var emptyName = "";

        // Act
        var result = ProductValidator.ValidateName(emptyName);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("required", result.ErrorMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ValidateName_WhitespaceName_ReturnsFalse()
    {
        // Arrange
        var whitespaceName = "   ";

        // Act
        var result = ProductValidator.ValidateName(whitespaceName);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("required", result.ErrorMessage);
    }

    [Fact]
    public void ValidateName_TooLongName_ReturnsFalse()
    {
        // Arrange
        var tooLongName = new string('A', 101); // 101 characters

        // Act
        var result = ProductValidator.ValidateName(tooLongName);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("100 characters", result.ErrorMessage);
    }

    [Fact]
    public void ValidateName_ExactlyMaxLength_ReturnsTrue()
    {
        // Arrange
        var maxLengthName = new string('A', 100); // Exactly 100 characters

        // Act
        var result = ProductValidator.ValidateName(maxLengthName);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ValidateName_DangerousContent_ReturnsFalse()
    {
        // Arrange
        var dangerousName = "<script>alert('xss')</script>";

        // Act
        var result = ProductValidator.ValidateName(dangerousName);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("invalid characters", result.ErrorMessage, StringComparison.OrdinalIgnoreCase);
    }

    #endregion

    #region Description Validation Tests

    [Fact]
    public void ValidateDescription_ValidDescription_ReturnsTrue()
    {
        // Arrange
        var validDesc = "This is a valid product description.";

        // Act
        var result = ProductValidator.ValidateDescription(validDesc);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ValidateDescription_EmptyDescription_ReturnsTrue()
    {
        // Arrange - Description is optional
        var emptyDesc = "";

        // Act
        var result = ProductValidator.ValidateDescription(emptyDesc);

        // Assert
        Assert.True(result.IsValid); // Description is optional
    }

    [Fact]
    public void ValidateDescription_TooLong_ReturnsFalse()
    {
        // Arrange
        var tooLongDesc = new string('A', 501); // 501 characters

        // Act
        var result = ProductValidator.ValidateDescription(tooLongDesc);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("500 characters", result.ErrorMessage);
    }

    [Fact]
    public void ValidateDescription_ExactlyMaxLength_ReturnsTrue()
    {
        // Arrange
        var maxLengthDesc = new string('A', 500);

        // Act
        var result = ProductValidator.ValidateDescription(maxLengthDesc);

        // Assert
        Assert.True(result.IsValid);
    }

    #endregion

    #region Price Validation Tests

    [Theory]
    [InlineData(0.01)]
    [InlineData(1.00)]
    [InlineData(100.00)]
    [InlineData(999999.99)]
    public void ValidatePrice_ValidPrices_ReturnsTrue(decimal price)
    {
        // Act
        var result = ProductValidator.ValidatePrice(price);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void ValidatePrice_ZeroOrNegative_ReturnsFalse(decimal price)
    {
        // Act
        var result = ProductValidator.ValidatePrice(price);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("greater than zero", result.ErrorMessage);
    }

    [Fact]
    public void ValidatePrice_TooHigh_ReturnsFalse()
    {
        // Arrange
        var tooHighPrice = 1000001m;

        // Act
        var result = ProductValidator.ValidatePrice(tooHighPrice);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("1,000,000", result.ErrorMessage);
    }

    [Fact]
    public void ValidatePrice_ExactlyMaxPrice_ReturnsTrue()
    {
        // Arrange
        var maxPrice = 1000000m;

        // Act
        var result = ProductValidator.ValidatePrice(maxPrice);

        // Assert
        Assert.True(result.IsValid);
    }

    #endregion

    #region Category Validation Tests

    [Fact]
    public void ValidateCategory_ValidCategory_ReturnsTrue()
    {
        // Arrange
        var validCategory = "Electronics";

        // Act
        var result = ProductValidator.ValidateCategory(validCategory);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ValidateCategory_EmptyCategory_ReturnsTrue()
    {
        // Arrange - Category is optional
        var emptyCategory = "";

        // Act
        var result = ProductValidator.ValidateCategory(emptyCategory);

        // Assert
        Assert.True(result.IsValid); // Category is optional
    }

    [Fact]
    public void ValidateCategory_TooLong_ReturnsFalse()
    {
        // Arrange
        var tooLongCategory = new string('A', 51);

        // Act
        var result = ProductValidator.ValidateCategory(tooLongCategory);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("50 characters", result.ErrorMessage);
    }

    [Fact]
    public void ValidateCategory_ExactlyMaxLength_ReturnsTrue()
    {
        // Arrange
        var maxLengthCategory = new string('A', 50);

        // Act
        var result = ProductValidator.ValidateCategory(maxLengthCategory);

        // Assert
        Assert.True(result.IsValid);
    }

    #endregion

    #region Stock Quantity Validation Tests

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(1000000)]
    public void ValidateStockQuantity_ValidQuantities_ReturnsTrue(int quantity)
    {
        // Act
        var result = ProductValidator.ValidateStockQuantity(quantity);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    public void ValidateStockQuantity_Negative_ReturnsFalse(int quantity)
    {
        // Act
        var result = ProductValidator.ValidateStockQuantity(quantity);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("negative", result.ErrorMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ValidateStockQuantity_TooHigh_ReturnsFalse()
    {
        // Arrange
        var tooHighQuantity = 1000001;

        // Act
        var result = ProductValidator.ValidateStockQuantity(tooHighQuantity);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("1,000,000", result.ErrorMessage);
    }

    #endregion

    #region Complete Product Validation Tests

    [Fact]
    public void ValidateProduct_AllFieldsValid_ReturnsTrue()
    {
        // Arrange
        var name = "Valid Product";
        var description = "Valid description";
        var price = 99.99m;
        var category = "Electronics";
        var stock = 100;

        // Act
        var result = ProductValidator.ValidateProduct(name, description, price, category, stock);

        // Assert
        Assert.True(result.IsValid);
        Assert.Equal(string.Empty, result.ErrorMessage);
    }

    [Fact]
    public void ValidateProduct_InvalidName_ReturnsFalseWithNameError()
    {
        // Arrange
        var invalidName = "";
        var validDescription = "Valid description";
        var validPrice = 99.99m;
        var validCategory = "Electronics";
        var validStock = 100;

        // Act
        var result = ProductValidator.ValidateProduct(
            invalidName, validDescription, validPrice, validCategory, validStock);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("name", result.ErrorMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ValidateProduct_InvalidPrice_ReturnsFalseWithPriceError()
    {
        // Arrange
        var validName = "Product";
        var validDescription = "Description";
        var invalidPrice = -10m;
        var validCategory = "Electronics";
        var validStock = 100;

        // Act
        var result = ProductValidator.ValidateProduct(
            validName, validDescription, invalidPrice, validCategory, validStock);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("price", result.ErrorMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ValidateProduct_InvalidStock_ReturnsFalseWithStockError()
    {
        // Arrange
        var validName = "Product";
        var validDescription = "Description";
        var validPrice = 99.99m;
        var validCategory = "Electronics";
        var invalidStock = -10;

        // Act
        var result = ProductValidator.ValidateProduct(
            validName, validDescription, validPrice, validCategory, invalidStock);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("stock", result.ErrorMessage, StringComparison.OrdinalIgnoreCase);
    }

    #endregion
}
