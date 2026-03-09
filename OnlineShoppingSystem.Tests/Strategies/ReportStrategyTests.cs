using Moq;
using OnlineShoppingSystem.Enums;
using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Models;
using OnlineShoppingSystem.Strategies;
using Xunit;

namespace OnlineShoppingSystem.Tests.Strategies;

/// <summary>
/// Unit tests for Report Strategies (Strategy Pattern)
/// </summary>
public class ReportStrategyTests
{
    [Fact]
    public void SalesSummaryStrategy_GetReportName_ReturnsCorrectName()
    {
        // Arrange
        var mockOrderService = new Mock<IOrderService>();
        var strategy = new SalesSummaryStrategy(mockOrderService.Object);

        // Act
        var name = strategy.GetReportName();

        // Assert
        Assert.Equal("Sales Summary", name);
    }

    [Fact]
    public void TopProductsStrategy_GetReportName_ReturnsNameWithLimit()
    {
        // Arrange
        var mockOrderService = new Mock<IOrderService>();
        var limit = 10;
        var strategy = new TopProductsStrategy(mockOrderService.Object, limit);

        // Act
        var name = strategy.GetReportName();

        // Assert
        Assert.Equal("Top 10 Products", name);
    }

    [Fact]
    public void TopProductsStrategy_DifferentLimits_ReturnsCorrectNames()
    {
        // Arrange
        var mockOrderService = new Mock<IOrderService>();
        var strategy5 = new TopProductsStrategy(mockOrderService.Object, 5);
        var strategy20 = new TopProductsStrategy(mockOrderService.Object, 20);

        // Act & Assert
        Assert.Equal("Top 5 Products", strategy5.GetReportName());
        Assert.Equal("Top 20 Products", strategy20.GetReportName());
    }

    [Fact]
    public void SalesByCategoryStrategy_GetReportName_ReturnsCorrectName()
    {
        // Arrange
        var mockOrderService = new Mock<IOrderService>();
        var mockProductService = new Mock<IProductService>();
        var strategy = new SalesByCategoryStrategy(
            mockOrderService.Object, 
            mockProductService.Object);

        // Act
        var name = strategy.GetReportName();

        // Assert
        Assert.Equal("Sales by Category", name);
    }

    [Fact]
    public void AllStrategies_ImplementIReportStrategy()
    {
        // Arrange
        var mockOrderService = new Mock<IOrderService>();
        var mockProductService = new Mock<IProductService>();

        // Act & Assert
        Assert.IsAssignableFrom<IReportStrategy>(
            new SalesSummaryStrategy(mockOrderService.Object));
        
        Assert.IsAssignableFrom<IReportStrategy>(
            new TopProductsStrategy(mockOrderService.Object, 10));
        
        Assert.IsAssignableFrom<IReportStrategy>(
            new SalesByCategoryStrategy(mockOrderService.Object, mockProductService.Object));
    }

    [Fact]
    public void TopProductsStrategy_DifferentLimits_CreatesWithCorrectLimit()
    {
        // Arrange
        var mockOrderService = new Mock<IOrderService>();

        // Act
        var strategy5 = new TopProductsStrategy(mockOrderService.Object, 5);
        var strategy50 = new TopProductsStrategy(mockOrderService.Object, 50);

        // Assert - Verify names reflect the limits
        Assert.Contains("5", strategy5.GetReportName());
        Assert.Contains("50", strategy50.GetReportName());
    }

    [Fact]
    public void SalesSummaryStrategy_Constructor_AcceptsIOrderService()
    {
        // Arrange
        var mockOrderService = new Mock<IOrderService>();

        // Act
        var strategy = new SalesSummaryStrategy(mockOrderService.Object);

        // Assert
        Assert.NotNull(strategy);
    }

    [Fact]
    public void SalesByCategoryStrategy_Constructor_AcceptsBothServices()
    {
        // Arrange
        var mockOrderService = new Mock<IOrderService>();
        var mockProductService = new Mock<IProductService>();

        // Act
        var strategy = new SalesByCategoryStrategy(mockOrderService.Object, mockProductService.Object);

        // Assert
        Assert.NotNull(strategy);
    }
}
