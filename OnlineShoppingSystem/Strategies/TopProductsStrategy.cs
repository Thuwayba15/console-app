using OnlineShoppingSystem.Helpers;
using OnlineShoppingSystem.Interfaces;

namespace OnlineShoppingSystem.Strategies;

/// <summary>
/// Strategy for generating top products reports
/// Shows best-selling products ranked by quantity sold
/// </summary>
public class TopProductsStrategy : IReportStrategy
{
    private readonly IOrderService _orderService;
    private readonly int _limit;

    public TopProductsStrategy(IOrderService orderService, int limit)
    {
        _orderService = orderService;
        _limit = limit;
    }

    public void GenerateReport()
    {
        ConsoleHelper.DisplayHeader("TOP SELLING PRODUCTS");
        var orders = _orderService.GetAllOrders();
        ReportDisplayHelper.DisplayTopProducts(orders, _limit);
    }

    public string GetReportName()
    {
        return $"Top {_limit} Products";
    }
}
