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

        // Load data from JSON files if they exist, otherwise initialize with seed data
        try
        {
            persistenceService.LoadData();
            
            // If no data was loaded (empty JSON files or first run), initialize with seed data
            if (!AppDataStore.Instance.Users.Any())
            {
                SeedData.Initialize();
                Console.WriteLine("Initialized with seed data.");
            }
            else
            {
                Console.WriteLine("Loaded existing data from storage.");
            }
        }
        catch
        {
            // If loading fails (files don't exist yet), initialize with seed data
            SeedData.Initialize();
            Console.WriteLine("Initialized with seed data.");
        }

        // Start the main menu with all necessary services
        var mainMenu = new MainMenu(
            authService,
            productService,
            cartService,
            orderService,
            paymentService,
            reviewService,
            reportService,
            persistenceService);
        mainMenu.Show();

        // Save data before exit
        persistenceService.SaveData();
    }
}
