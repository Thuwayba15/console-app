namespace OnlineShoppingSystem.Interfaces;

/// <summary>
/// Interface for generating reports
/// </summary>
public interface IReportService
{
    /// <summary>
    /// Generate a sales report showing total revenue and orders
    /// </summary>
    void GenerateSalesReport();

    /// <summary>
    /// Generate a report of products with reviews
    /// </summary>
    void GenerateProductReviewsReport();
}
