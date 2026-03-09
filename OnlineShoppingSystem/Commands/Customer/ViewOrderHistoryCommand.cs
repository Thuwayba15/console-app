using OnlineShoppingSystem.Helpers;
using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Commands.Customer;

/// <summary>
/// Command for viewing order history
/// </summary>
public class ViewOrderHistoryCommand : ICommand
{
    private readonly Models.Customer _customer;
    private readonly IOrderService _orderService;

    public ViewOrderHistoryCommand(Models.Customer customer, IOrderService orderService)
    {
        _customer = customer;
        _orderService = orderService;
    }

    public void Execute()
    {
        ConsoleHelper.DisplayHeader("ORDER HISTORY");

        var orders = _orderService.GetCustomerOrders(_customer.Id);

        if (!orders.Any())
        {
            ConsoleHelper.DisplayWarning("You have no orders yet.");
            ConsoleHelper.PauseForUser();
            return;
        }

        OrderDisplayHelper.DisplayOrderHistory(orders);
        ConsoleHelper.PauseForUser();
    }

    public string GetName() => "View Order History";
}
