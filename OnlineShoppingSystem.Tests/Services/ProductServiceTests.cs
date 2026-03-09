using OnlineShoppingSystem.Data;
using OnlineShoppingSystem.Models;
using OnlineShoppingSystem.Services;
using Xunit;

namespace OnlineShoppingSystem.Tests.Services;

/// <summary>
/// Unit tests for ProductService
/// Uses Sequential collection to prevent race conditions with singleton AppDataStore
/// </summary>
[Collection("Sequential")]
public class ProductServiceTests : IDisposable
{
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        ResetDataStore();
        _productService = new ProductService();
    }

    public void Dispose()
    {
        ResetDataStore();
    }

    private void ResetDataStore()
    {
        var dataStore = AppDataStore.Instance;
        dataStore.Products.Clear();
    }

    #region GetAllProducts Tests

    [Fact]
    public void GetAllProducts_EmptyStore_ReturnsEmptyList()
    {
        // Act
        var products = _productService.GetAllProducts();

        // Assert
        Assert.NotNull(products);
        Assert.Empty(products);
    }

    [Fact]
    public void GetAllProducts_WithProducts_ReturnsAllProducts()
    {
        // Arrange
        _productService.AddProduct("Product1", "Desc1", 10m, 5, "Cat1");
        _productService.AddProduct("Product2", "Desc2", 20m, 10, "Cat2");

        // Act
        var products = _productService.GetAllProducts();

        // Assert
        Assert.Equal(2, products.Count);
    }

    #endregion

    #region GetProductById Tests

    [Fact]
    public void GetProductById_ExistingProduct_ReturnsProduct()
    {
        // Arrange
        var addedProduct = _productService.AddProduct("Test", "Desc", 10m, 5, "Cat");

        // Act
        var product = _productService.GetProductById(addedProduct.Id);

        // Assert
        Assert.NotNull(product);
        Assert.Equal(addedProduct.Id, product.Id);
        Assert.Equal("Test", product.Name);
    }

    [Fact]
    public void GetProductById_NonExistingProduct_ReturnsNull()
    {
        // Act
        var product = _productService.GetProductById(999);

        // Assert
        Assert.Null(product);
    }

    #endregion

    #region AddProduct Tests

    [Fact]
    public void AddProduct_ValidData_AddsProductSuccessfully()
    {
        // Act
        var product = _productService.AddProduct("Laptop", "Gaming laptop", 1500m, 10, "Electronics");

        // Assert
        Assert.NotNull(product);
        Assert.Equal("Laptop", product.Name);
        Assert.Equal("Gaming laptop", product.Description);
        Assert.Equal(1500m, product.Price);
        Assert.Equal(10, product.StockQuantity);
        Assert.Equal("Electronics", product.Category);
    }

    [Fact]
    public void AddProduct_TrimsWhitespace_StoresCleanData()
    {
        // Act
        var product = _productService.AddProduct("  Laptop  ", "  Description  ", 100m, 5, "  Electronics  ");

        // Assert
        Assert.Equal("Laptop", product.Name);
        Assert.Equal("Description", product.Description);
        Assert.Equal("Electronics", product.Category);
    }

    [Fact]
    public void AddProduct_EmptyName_ThrowsException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            _productService.AddProduct("", "Desc", 10m, 5, "Cat"));
        
        Assert.Contains("name", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void AddProduct_NameTooLong_ThrowsException()
    {
        // Arrange
        var tooLongName = new string('A', 101);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            _productService.AddProduct(tooLongName, "Desc", 10m, 5, "Cat"));
        
        Assert.Contains("100 characters", exception.Message);
    }

    [Fact]
    public void AddProduct_NegativePrice_ThrowsException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            _productService.AddProduct("Product", "Desc", -10m, 5, "Cat"));
        
        Assert.Contains("greater than zero", exception.Message);
    }

    [Fact]
    public void AddProduct_ZeroPrice_ThrowsException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            _productService.AddProduct("Product", "Desc", 0m, 5, "Cat"));
        
        Assert.Contains("greater than zero", exception.Message);
    }

    [Fact]
    public void AddProduct_PriceTooHigh_ThrowsException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            _productService.AddProduct("Product", "Desc", 1000001m, 5, "Cat"));
        
        Assert.Contains("1,000,000", exception.Message);
    }

    [Fact]
    public void AddProduct_NegativeStock_ThrowsException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            _productService.AddProduct("Product", "Desc", 10m, -5, "Cat"));
        
        Assert.Contains("negative", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void AddProduct_StockTooHigh_ThrowsException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            _productService.AddProduct("Product", "Desc", 10m, 1000001, "Cat"));
        
        Assert.Contains("1,000,000", exception.Message);
    }

    [Fact]
    public void AddProduct_GeneratesUniqueIds()
    {
        // Act
        var product1 = _productService.AddProduct("P1", "D1", 10m, 5, "C1");
        var product2 = _productService.AddProduct("P2", "D2", 20m, 10, "C2");

        // Assert
        Assert.NotEqual(product1.Id, product2.Id);
        Assert.True(product2.Id > product1.Id);
    }

    #endregion

    #region UpdateProduct Tests

    [Fact]
    public void UpdateProduct_ExistingProduct_UpdatesSuccessfully()
    {
        // Arrange
        var product = _productService.AddProduct("Old Name", "Old Desc", 10m, 5, "Old Cat");

        // Act
        var result = _productService.UpdateProduct(product.Id, "New Name", "New Desc", 20m, 10, "New Cat");

        // Assert
        Assert.True(result);
        var updated = _productService.GetProductById(product.Id);
        Assert.Equal("New Name", updated!.Name);
        Assert.Equal("New Desc", updated.Description);
        Assert.Equal(20m, updated.Price);
        Assert.Equal(10, updated.StockQuantity);
        Assert.Equal("New Cat", updated.Category);
    }

    [Fact]
    public void UpdateProduct_NonExistingProduct_ReturnsFalse()
    {
        // Act
        var result = _productService.UpdateProduct(999, "Name", "Desc", 10m, 5, "Cat");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void UpdateProduct_InvalidName_ThrowsException()
    {
        // Arrange
        var product = _productService.AddProduct("Product", "Desc", 10m, 5, "Cat");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            _productService.UpdateProduct(product.Id, "", "Desc", 10m, 5, "Cat"));
        
        Assert.Contains("name", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    #endregion

    #region DeleteProduct Tests

    [Fact]
    public void DeleteProduct_ExistingProduct_DeletesSuccessfully()
    {
        // Arrange
        var product = _productService.AddProduct("Product", "Desc", 10m, 5, "Cat");

        // Act
        var result = _productService.DeleteProduct(product.Id);

        // Assert
        Assert.True(result);
        Assert.Null(_productService.GetProductById(product.Id));
    }

    [Fact]
    public void DeleteProduct_NonExistingProduct_ReturnsFalse()
    {
        // Act
        var result = _productService.DeleteProduct(999);

        // Assert
        Assert.False(result);
    }

    #endregion

    #region RestockProduct Tests

    [Fact]
    public void RestockProduct_ValidQuantity_IncreasesStock()
    {
        // Arrange
        var product = _productService.AddProduct("Product", "Desc", 10m, 5, "Cat");
        var originalStock = product.StockQuantity;

        // Act
        var result = _productService.RestockProduct(product.Id, 10);

        // Assert
        Assert.True(result);
        var updated = _productService.GetProductById(product.Id);
        Assert.Equal(originalStock + 10, updated!.StockQuantity);
    }

    [Fact]
    public void RestockProduct_ZeroQuantity_ThrowsException()
    {
        // Arrange
        var product = _productService.AddProduct("Product", "Desc", 10m, 5, "Cat");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            _productService.RestockProduct(product.Id, 0));
        
        Assert.Contains("greater than zero", exception.Message);
    }

    [Fact]
    public void RestockProduct_NegativeQuantity_ThrowsException()
    {
        // Arrange
        var product = _productService.AddProduct("Product", "Desc", 10m, 5, "Cat");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            _productService.RestockProduct(product.Id, -10));
        
        Assert.Contains("greater than zero", exception.Message);
    }

    [Fact]
    public void RestockProduct_NonExistingProduct_ReturnsFalse()
    {
        // Act
        var result = _productService.RestockProduct(999, 10);

        // Assert
        Assert.False(result);
    }

    #endregion

    #region SearchProducts Tests

    [Fact]
    public void SearchProducts_EmptyTerm_ReturnsAllProducts()
    {
        // Arrange
        _productService.AddProduct("Laptop", "Gaming laptop", 1500m, 10, "Electronics");
        _productService.AddProduct("Mouse", "Gaming mouse", 50m, 20, "Accessories");

        // Act
        var results = _productService.SearchProducts("");

        // Assert
        Assert.Equal(2, results.Count);
    }

    [Fact]
    public void SearchProducts_MatchingName_ReturnsMatchingProducts()
    {
        // Arrange
        _productService.AddProduct("Laptop", "Gaming laptop", 1500m, 10, "Electronics");
        _productService.AddProduct("Desktop", "Gaming desktop", 2000m, 5, "Electronics");
        _productService.AddProduct("Mouse", "Gaming mouse", 50m, 20, "Accessories");

        // Act
        var results = _productService.SearchProducts("Laptop");

        // Assert
        Assert.Single(results);
        Assert.Equal("Laptop", results[0].Name);
    }

    [Fact]
    public void SearchProducts_MatchingDescription_ReturnsMatchingProducts()
    {
        // Arrange
        _productService.AddProduct("Laptop", "Gaming laptop", 1500m, 10, "Electronics");
        _productService.AddProduct("Mouse", "Office mouse", 30m, 20, "Accessories");

        // Act
        var results = _productService.SearchProducts("Gaming");

        // Assert
        Assert.Single(results);
        Assert.Contains("Gaming", results[0].Description);
    }

    [Fact]
    public void SearchProducts_MatchingCategory_ReturnsMatchingProducts()
    {
        // Arrange
        _productService.AddProduct("Laptop", "Gaming laptop", 1500m, 10, "Electronics");
        _productService.AddProduct("Phone", "Smartphone", 800m, 15, "Electronics");
        _productService.AddProduct("Mouse", "Gaming mouse", 50m, 20, "Accessories");

        // Act
        var results = _productService.SearchProducts("Electronics");

        // Assert
        Assert.Equal(2, results.Count);
        Assert.All(results, p => Assert.Equal("Electronics", p.Category));
    }

    [Fact]
    public void SearchProducts_CaseInsensitive_FindsMatches()
    {
        // Arrange
        _productService.AddProduct("Laptop", "Gaming laptop", 1500m, 10, "Electronics");

        // Act
        var results1 = _productService.SearchProducts("LAPTOP");
        var results2 = _productService.SearchProducts("laptop");
        var results3 = _productService.SearchProducts("LaPtOp");

        // Assert
        Assert.Single(results1);
        Assert.Single(results2);
        Assert.Single(results3);
    }

    [Fact]
    public void SearchProducts_NoMatches_ReturnsEmptyList()
    {
        // Arrange
        _productService.AddProduct("Laptop", "Gaming laptop", 1500m, 10, "Electronics");

        // Act
        var results = _productService.SearchProducts("NonExistent");

        // Assert
        Assert.Empty(results);
    }

    #endregion

    #region GetLowStockProducts Tests

    [Fact]
    public void GetLowStockProducts_WithLowStockItems_ReturnsFilteredList()
    {
        // Arrange
        _productService.AddProduct("P1", "Desc", 10m, 5, "Cat");   // Low stock
        _productService.AddProduct("P2", "Desc", 10m, 8, "Cat");   // Low stock
        _productService.AddProduct("P3", "Desc", 10m, 15, "Cat");  // Not low stock

        // Act
        var lowStock = _productService.GetLowStockProducts(10);

        // Assert
        Assert.Equal(2, lowStock.Count);
        Assert.All(lowStock, p => Assert.True(p.StockQuantity <= 10));
    }

    [Fact]
    public void GetLowStockProducts_OrderedByQuantity_ReturnsAscendingOrder()
    {
        // Arrange
        _productService.AddProduct("P1", "Desc", 10m, 10, "Cat");
        _productService.AddProduct("P2", "Desc", 10m, 2, "Cat");
        _productService.AddProduct("P3", "Desc", 10m, 5, "Cat");

        // Act
        var lowStock = _productService.GetLowStockProducts(10);

        // Assert
        Assert.Equal(3, lowStock.Count);
        Assert.Equal(2, lowStock[0].StockQuantity);
        Assert.Equal(5, lowStock[1].StockQuantity);
        Assert.Equal(10, lowStock[2].StockQuantity);
    }

    [Fact]
    public void GetLowStockProducts_NoLowStock_ReturnsEmptyList()
    {
        // Arrange
        _productService.AddProduct("P1", "Desc", 10m, 50, "Cat");
        _productService.AddProduct("P2", "Desc", 10m, 100, "Cat");

        // Act
        var lowStock = _productService.GetLowStockProducts(10);

        // Assert
        Assert.Empty(lowStock);
    }

    #endregion
}
