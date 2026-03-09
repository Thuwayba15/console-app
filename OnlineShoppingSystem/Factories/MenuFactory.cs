using OnlineShoppingSystem.Enums;
using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Menus;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Factories;

/// <summary>
/// Factory for creating menu objects based on user role
/// Implements the Factory Pattern to encapsulate menu creation logic
/// This simplifies the routing logic and makes it easier to add new user roles
/// </summary>
public static class MenuFactory
{
    /// <summary>
    /// Create the appropriate menu for the given user
    /// </summary>
    /// <param name="user">The logged-in user</param>
    /// <param name="productService">Product service dependency</param>
    /// <param name="cartService">Cart service dependency</param>
    /// <param name="orderService">Order service dependency</param>
    /// <param name="paymentService">Payment service dependency</param>
    /// <param name="reviewService">Review service dependency</param>
    /// <param name="reportService">Report service dependency</param>
    /// <param name="persistenceService">Persistence service dependency</param>
    /// <returns>A menu object appropriate for the user's role</returns>
    public static object CreateMenu(
        User user,
        IProductService productService,
        ICartService cartService,
        IOrderService orderService,
        IPaymentService paymentService,
        IReviewService reviewService,
        IReportService reportService,
        IPersistenceService persistenceService)
    {
        // Factory Pattern: Create the appropriate menu based on user role
        return user.Role switch
        {
            UserRole.Customer => CreateCustomerMenu(
                (Customer)user,
                productService,
                cartService,
                orderService,
                paymentService,
                reviewService,
                persistenceService),

            UserRole.Administrator => CreateAdministratorMenu(
                (Administrator)user,
                productService,
                orderService,
                reportService,
                persistenceService),

            _ => throw new ArgumentException($"Unknown user role: {user.Role}")
        };
    }

    /// <summary>
    /// Create a customer menu with all required dependencies
    /// </summary>
    private static CustomerMenu CreateCustomerMenu(
        Customer customer,
        IProductService productService,
        ICartService cartService,
        IOrderService orderService,
        IPaymentService paymentService,
        IReviewService reviewService,
        IPersistenceService persistenceService)
    {
        return new CustomerMenu(
            customer,
            productService,
            cartService,
            orderService,
            paymentService,
            reviewService,
            persistenceService);
    }

    /// <summary>
    /// Create an administrator menu with all required dependencies
    /// </summary>
    private static AdministratorMenu CreateAdministratorMenu(
        Administrator admin,
        IProductService productService,
        IOrderService orderService,
        IReportService reportService,
        IPersistenceService persistenceService)
    {
        return new AdministratorMenu(
            admin,
            productService,
            orderService,
            reportService,
            persistenceService);
    }
}
