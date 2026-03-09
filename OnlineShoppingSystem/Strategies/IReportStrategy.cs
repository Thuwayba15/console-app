namespace OnlineShoppingSystem.Strategies;

/// <summary>
/// Strategy interface for different types of reports
/// Implements the Strategy Pattern to allow different report generation behaviors
/// </summary>
public interface IReportStrategy
{
    /// <summary>
    /// Generate and display the report
    /// </summary>
    void GenerateReport();

    /// <summary>
    /// Get the name of the report
    /// </summary>
    string GetReportName();
}
