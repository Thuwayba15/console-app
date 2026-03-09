using OnlineShoppingSystem.Helpers;
using OnlineShoppingSystem.Interfaces;

namespace OnlineShoppingSystem.Strategies;

/// <summary>
/// Strategy for generating sales by category reports
/// Shows revenue breakdown by product category
/// </summary>
public class SalesByCategoryStrategy : IReportStrategy
{
    private readonly IOrderService _orderService;
    private readonly IProductService _productService;

    public SalesByCategoryStrategy(IOrderService orderService, IProductService productService)
    {
        _orderService = orderService;
        _productService = productService;
    }

    public void GenerateReport()
    {
        ConsoleHelper.DisplayHeader("SALES BY CATEGORY");
        var orders = _orderService.GetAllOrders();
        var products = _productService.GetAllProducts();
        ReportDisplayHelper.DisplaySalesByCategory(orders, products);
    }

    public string GetReportName()
    {
        return "Sales by Category";
    }
}
