using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Services;

namespace OnlineShoppingSystem.Configuration;

/// <summary>
/// Centralizes service initialization and dependency configuration
/// This follows the Dependency Injection pattern and makes Program.cs cleaner
/// </summary>
public static class ServiceConfigurator
{
    /// <summary>
    /// Configure and initialize all application services
    /// Returns a tuple of all configured services
    /// </summary>
    public static ServiceContainer ConfigureServices()
    {
        // Initialize services in correct dependency order
        var authService = new AuthService();
        var productService = new ProductService();
        var paymentService = new PaymentService();
        var cartService = new CartService(productService);
        var orderService = new OrderService(cartService, productService, paymentService);
        var reviewService = new ReviewService();
        var reportService = new ReportService();
        var persistenceService = new PersistenceService();

        return new ServiceContainer
        {
            AuthService = authService,
            ProductService = productService,
            PaymentService = paymentService,
            CartService = cartService,
            OrderService = orderService,
            ReviewService = reviewService,
            ReportService = reportService,
            PersistenceService = persistenceService
        };
    }
}

/// <summary>
/// Container for all application services
/// Makes it easy to pass services around without multiple parameters
/// </summary>
public class ServiceContainer
{
    public IAuthService AuthService { get; init; } = null!;
    public IProductService ProductService { get; init; } = null!;
    public IPaymentService PaymentService { get; init; } = null!;
    public ICartService CartService { get; init; } = null!;
    public IOrderService OrderService { get; init; } = null!;
    public IReviewService ReviewService { get; init; } = null!;
    public IReportService ReportService { get; init; } = null!;
    public IPersistenceService PersistenceService { get; init; } = null!;
}
