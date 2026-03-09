using OnlineShoppingSystem.Helpers;
using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Commands.Customer;

/// <summary>
/// Command for tracking specific orders
/// </summary>
public class TrackOrdersCommand : ICommand
{
    private readonly Models.Customer _customer;
    private readonly IOrderService _orderService;

    public TrackOrdersCommand(Models.Customer customer, IOrderService orderService)
    {
        _customer = customer;
        _orderService = orderService;
    }

    public void Execute()
    {
        ConsoleHelper.DisplayHeader("TRACK ORDERS");

        var orders = _orderService.GetCustomerOrders(_customer.Id);

        if (!orders.Any())
        {
            ConsoleHelper.DisplayWarning("You have no orders to track.");
            ConsoleHelper.PauseForUser();
            return;
        }

        OrderDisplayHelper.DisplayOrderSummaryTable(orders);

        var orderId = InputHelper.ReadInt("Enter Order ID to view details (0 to cancel): ");

        if (orderId == 0) return;

        var selectedOrder = orders.FirstOrDefault(o => o.Id == orderId);

        if (selectedOrder == null)
        {
            ConsoleHelper.DisplayError("Order not found.");
            ConsoleHelper.PauseForUser();
            return;
        }

        OrderDisplayHelper.DisplayOrderDetails(selectedOrder);
        ConsoleHelper.PauseForUser();
    }

    public string GetName() => "Track Orders";
}
