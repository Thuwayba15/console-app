using OnlineShoppingSystem.Helpers;
using OnlineShoppingSystem.Interfaces;

namespace OnlineShoppingSystem.Strategies;

/// <summary>
/// Strategy for generating sales summary reports
/// Shows total orders, revenue, items sold, and average order value
/// </summary>
public class SalesSummaryStrategy : IReportStrategy
{
    private readonly IOrderService _orderService;

    public SalesSummaryStrategy(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public void GenerateReport()
    {
        ConsoleHelper.DisplayHeader("SALES SUMMARY");
        var orders = _orderService.GetAllOrders();
        ReportDisplayHelper.DisplaySalesSummary(orders);
    }

    public string GetReportName()
    {
        return "Sales Summary";
    }
}
