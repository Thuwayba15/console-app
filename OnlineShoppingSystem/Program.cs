using OnlineShoppingSystem.Data;
using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Services;
using OnlineShoppingSystem.Menus;

namespace OnlineShoppingSystem;

/// <summary>
/// Entry point for the Online Shopping Backend System
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        // Initialize services
        IAuthService authService = new AuthService();
        IProductService productService = new ProductService();
        IPaymentService paymentService = new PaymentService();
        ICartService cartService = new CartService(productService);
        IOrderService orderService = new OrderService(cartService, productService, paymentService);
        IReviewService reviewService = new ReviewService();
        IReportService reportService = new ReportService();
        IPersistenceService persistenceService = new PersistenceService();

        // Initialize data with seed data
        SeedData.Initialize();

        // Start the main menu
        var mainMenu = new MainMenu(authService);
        mainMenu.Show();

        // Save data before exit
        persistenceService.SaveData();
    }
}
