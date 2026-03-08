using OnlineShoppingSystem.Helpers;
using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Menus;

/// <summary>
/// Administrator menu for managing products, orders, and reports
/// </summary>
public class AdministratorMenu
{
    private readonly Administrator _admin;
    private readonly IProductService _productService;
    private readonly IOrderService _orderService;
    private readonly IReportService _reportService;

    public AdministratorMenu(
        Administrator admin,
        IProductService productService,
        IOrderService orderService,
        IReportService reportService)
    {
        _admin = admin;
        _productService = productService;
        _orderService = orderService;
        _reportService = reportService;
    }

    /// <summary>
    /// Display and handle the administrator menu
    /// </summary>
    public void Show()
    {
        while (true)
        {
            ConsoleHelper.DisplayHeader($"ADMINISTRATOR MENU - Welcome, {_admin.Username}!");
            Console.WriteLine("1. Add Product");
            Console.WriteLine("2. Update Product");
            Console.WriteLine("3. Delete Product");
            Console.WriteLine("4. Restock Product");
            Console.WriteLine("5. View Products");
            Console.WriteLine("6. View Orders");
            Console.WriteLine("7. Update Order Status");
            Console.WriteLine("8. View Low Stock Products");
            Console.WriteLine("9. Generate Sales Reports");
            Console.WriteLine("10. Logout");

            var choice = InputHelper.ReadMenuChoice(1, 10);

            switch (choice)
            {
                case 1:
                    ConsoleHelper.DisplayInfo("Add Product feature coming in next milestone.");
                    ConsoleHelper.PauseForUser();
                    break;
                case 2:
                    ConsoleHelper.DisplayInfo("Update Product feature coming in next milestone.");
                    ConsoleHelper.PauseForUser();
                    break;
                case 3:
                    ConsoleHelper.DisplayInfo("Delete Product feature coming in next milestone.");
                    ConsoleHelper.PauseForUser();
                    break;
                case 4:
                    ConsoleHelper.DisplayInfo("Restock Product feature coming in next milestone.");
                    ConsoleHelper.PauseForUser();
                    break;
                case 5:
                    ConsoleHelper.DisplayInfo("View Products feature coming in next milestone.");
                    ConsoleHelper.PauseForUser();
                    break;
                case 6:
                    ConsoleHelper.DisplayInfo("View Orders feature coming in next milestone.");
                    ConsoleHelper.PauseForUser();
                    break;
                case 7:
                    ConsoleHelper.DisplayInfo("Update Order Status feature coming in next milestone.");
                    ConsoleHelper.PauseForUser();
                    break;
                case 8:
                    ConsoleHelper.DisplayInfo("View Low Stock feature coming in next milestone.");
                    ConsoleHelper.PauseForUser();
                    break;
                case 9:
                    ConsoleHelper.DisplayInfo("Sales Reports feature coming in next milestone.");
                    ConsoleHelper.PauseForUser();
                    break;
                case 10:
                    ConsoleHelper.DisplaySuccess("Logged out successfully.");
                    ConsoleHelper.PauseForUser();
                    return;
            }
        }
    }
}
