using OnlineShoppingSystem.Data;
using OnlineShoppingSystem.Enums;
using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Services;

/// <summary>
/// Service for managing orders
/// </summary>
public class OrderService : IOrderService
{
    private readonly AppDataStore _dataStore;
    private readonly ICartService _cartService;
    private readonly IProductService _productService;
    private readonly IPaymentService _paymentService;

    public OrderService(ICartService cartService, IProductService productService, IPaymentService paymentService)
    {
        _dataStore = AppDataStore.Instance;
        _cartService = cartService;
        _productService = productService;
        _paymentService = paymentService;
    }

    public Order? CreateOrder(int customerId)
    {
        var cart = _cartService.GetCart(customerId);
        if (cart == null || !cart.Items.Any())
        {
            throw new InvalidOperationException("Cart is empty.");
        }

        // Verify stock availability for all items
        foreach (var cartItem in cart.Items)
        {
            var product = _productService.GetProductById(cartItem.ProductId);
            if (product == null)
            {
                throw new InvalidOperationException($"Product {cartItem.ProductName} no longer exists.");
            }

            if (product.StockQuantity < cartItem.Quantity)
            {
                throw new InvalidOperationException($"Insufficient stock for {product.Name}. Only {product.StockQuantity} available.");
            }
        }

        var totalAmount = cart.GetTotal();

        // Process payment
        var paymentSuccessful = _paymentService.ProcessPayment(customerId, 0, totalAmount);
        if (!paymentSuccessful)
        {
            throw new InvalidOperationException("Payment failed. Insufficient wallet balance.");
        }

        // Create order
        var order = new Order
        {
            Id = _dataStore.GetNextOrderId(),
            CustomerId = customerId,
            TotalAmount = totalAmount,
            Status = OrderStatus.Pending
        };

        // Convert cart items to order items and reduce stock
        foreach (var cartItem in cart.Items)
        {
            var orderItem = new OrderItem
            {
                ProductId = cartItem.ProductId,
                ProductName = cartItem.ProductName,
                Price = cartItem.Price,
                Quantity = cartItem.Quantity
            };
            order.Items.Add(orderItem);

            // Reduce stock
            var product = _productService.GetProductById(cartItem.ProductId);
            if (product != null)
            {
                product.StockQuantity -= cartItem.Quantity;
            }
        }

        _dataStore.Orders.Add(order);

        // Update customer's order list
        var customer = _dataStore.Users.OfType<Customer>().FirstOrDefault(c => c.Id == customerId);
        if (customer != null)
        {
            customer.OrderIds.Add(order.Id);
        }

        // Clear the cart
        _cartService.ClearCart(customerId);

        return order;
    }

    public List<Order> GetCustomerOrders(int customerId)
    {
        return _dataStore.Orders
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.OrderDate)
            .ToList();
    }

    public Order? GetOrderById(int orderId)
    {
        return _dataStore.Orders.FirstOrDefault(o => o.Id == orderId);
    }

    public List<Order> GetAllOrders()
    {
        return _dataStore.Orders
            .OrderByDescending(o => o.OrderDate)
            .ToList();
    }

    public bool UpdateOrderStatus(int orderId, OrderStatus newStatus)
    {
        var order = GetOrderById(orderId);
        if (order == null)
        {
            return false;
        }

        order.Status = newStatus;

        if (newStatus == OrderStatus.Delivered)
        {
            order.DeliveryDate = DateTime.Now;
        }

        return true;
    }
}
