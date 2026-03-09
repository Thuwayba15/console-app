using OnlineShoppingSystem.Strategies;

namespace OnlineShoppingSystem.Services;

/// <summary>
/// Service for generating reports using different strategies
/// Demonstrates the Strategy Pattern for report generation
/// </summary>
public class ReportGenerator
{
    /// <summary>
    /// Generate a report using the specified strategy
    /// Strategy Pattern: The algorithm (report type) is selected at runtime
    /// </summary>
    /// <param name="strategy">The report strategy to use</param>
    public void Generate(IReportStrategy strategy)
    {
        if (strategy == null)
        {
            throw new ArgumentNullException(nameof(strategy), "Report strategy cannot be null");
        }

        strategy.GenerateReport();
    }

    /// <summary>
    /// Generate a report with a custom title
    /// </summary>
    public void GenerateWithTitle(IReportStrategy strategy, string customTitle)
    {
        if (strategy == null)
        {
            throw new ArgumentNullException(nameof(strategy), "Report strategy cannot be null");
        }

        Console.WriteLine($"\n{'=',60}");
        Console.WriteLine($"  {customTitle}");
        Console.WriteLine($"{'=',60}\n");

        strategy.GenerateReport();
    }
}
