using OnlineShoppingSystem.Enums;
using OnlineShoppingSystem.Helpers;
using OnlineShoppingSystem.Interfaces;

namespace OnlineShoppingSystem.Commands.Admin;

public class ViewOrdersCommand : ICommand
{
    private readonly IOrderService _orderService;

    public ViewOrdersCommand(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public void Execute()
    {
        ConsoleHelper.DisplayHeader("ALL ORDERS");

        var orders = _orderService.GetAllOrders();

        if (!orders.Any())
        {
            ConsoleHelper.DisplayWarning("No orders found.");
        }
        else
        {
            var grouped = orders.GroupBy(o => o.Status).OrderBy(g => g.Key);

            foreach (var group in grouped)
            {
                Console.WriteLine($"\n=== {group.Key} Orders ({group.Count()}) ===");
                Console.WriteLine("{0,-10} {1,-15} {2,-20} {3,-12}", "Order ID", "Customer ID", "Date", "Total");
                Console.WriteLine(new string('-', 60));

                foreach (var order in group.OrderBy(o => o.OrderDate))
                {
                    Console.WriteLine("{0,-10} {1,-15} {2,-20} R{3,-11:F2}",
                        order.Id, order.CustomerId, order.OrderDate.ToString("yyyy-MM-dd HH:mm"), order.TotalAmount);
                }
            }

            Console.WriteLine($"\nTotal orders: {orders.Count}");
        }

        ConsoleHelper.PauseForUser();
    }

    public string GetName() => "View Orders";
}

public class UpdateOrderStatusCommand : ICommand
{
    private readonly IOrderService _orderService;
    private readonly IPersistenceService _persistenceService;

    public UpdateOrderStatusCommand(IOrderService orderService, IPersistenceService persistenceService)
    {
        _orderService = orderService;
        _persistenceService = persistenceService;
    }

    public void Execute()
    {
        ConsoleHelper.DisplayHeader("UPDATE ORDER STATUS");

        try
        {
            var orders = _orderService.GetAllOrders();

            if (!orders.Any())
            {
                ConsoleHelper.DisplayWarning("No orders found.");
                ConsoleHelper.PauseForUser();
                return;
            }

            Console.WriteLine("{0,-10} {1,-15} {2,-15}", "Order ID", "Customer ID", "Status");
            Console.WriteLine(new string('-', 45));
            foreach (var o in orders.OrderByDescending(o => o.OrderDate).Take(20))
            {
                Console.WriteLine("{0,-10} {1,-15} {2,-15}", o.Id, o.CustomerId, o.Status);
            }

            var orderId = InputHelper.ReadInt("\nEnter Order ID to update (0 to cancel): ");
            
            if (orderId == 0) return;

            var order = _orderService.GetOrderById(orderId);
            if (order == null)
            {
                ConsoleHelper.DisplayError("Order not found.");
                ConsoleHelper.PauseForUser();
                return;
            }

            Console.WriteLine($"\nOrder #{order.Id} - Current Status: {order.Status}");
            Console.WriteLine("\nSelect new status:");
            Console.WriteLine("1. Pending");
            Console.WriteLine("2. Processing");
            Console.WriteLine("3. Shipped");
            Console.WriteLine("4. Delivered");
            Console.WriteLine("5. Cancelled");

            var choice = InputHelper.ReadMenuChoice(1, 5);

            var newStatus = choice switch
            {
                1 => OrderStatus.Pending,
                2 => OrderStatus.Processing,
                3 => OrderStatus.Shipped,
                4 => OrderStatus.Delivered,
                5 => OrderStatus.Cancelled,
                _ => OrderStatus.Pending
            };

            if (_orderService.UpdateOrderStatus(orderId, newStatus))
            {
                ConsoleHelper.DisplaySuccess($"Order #{orderId} status updated to {newStatus}!");
                _persistenceService.SaveData();
            }
            else
            {
                ConsoleHelper.DisplayError("Failed to update status.");
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.DisplayError($"Error: {ex.Message}");
        }

        ConsoleHelper.PauseForUser();
    }

    public string GetName() => "Update Order Status";
}
