using OnlineShoppingSystem.Helpers;
using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Services;
using OnlineShoppingSystem.Strategies;

namespace OnlineShoppingSystem.Commands.Admin;

/// <summary>
/// Command for generating sales reports using Strategy Pattern
/// </summary>
public class GenerateReportsCommand : ICommand
{
    private readonly IOrderService _orderService;
    private readonly IProductService _productService;

    public GenerateReportsCommand(IOrderService orderService, IProductService productService)
    {
        _orderService = orderService;
        _productService = productService;
    }

    public void Execute()
    {
        ConsoleHelper.DisplayHeader("SALES REPORTS");

        try
        {
            Console.WriteLine("Select report type:");
            Console.WriteLine("1. Sales Summary");
            Console.WriteLine("2. Top Products");
            Console.WriteLine("3. Sales by Category");
            Console.WriteLine("4. Back to Menu");

            var choice = InputHelper.ReadMenuChoice(1, 4);

            IReportStrategy? strategy = null;
            var generator = new ReportGenerator();

            switch (choice)
            {
                case 1:
                    strategy = new SalesSummaryStrategy(_orderService);
                    break;
                case 2:
                    var limit = InputHelper.ReadPositiveInt("Enter number of top products: ");
                    strategy = new TopProductsStrategy(_orderService, limit);
                    break;
                case 3:
                    strategy = new SalesByCategoryStrategy(_orderService, _productService);
                    break;
                case 4:
                    return;
            }

            if (strategy != null)
            {
                generator.Generate(strategy);
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.DisplayError($"Error: {ex.Message}");
        }

        ConsoleHelper.PauseForUser();
    }

    public string GetName() => "Generate Reports";
}
