using OnlineShoppingSystem.Configuration;
using OnlineShoppingSystem.Data;
using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Menus;

namespace OnlineShoppingSystem;

/// <summary>
/// Application entry point for the Online Shopping System
/// 
/// Architecture Overview:
/// ----------------------
/// This application follows a layered architecture with design patterns:
/// 
/// Presentation Layer:
///   - Menus (CustomerMenu, AdministratorMenu, MainMenu)
///   - Commands (21 command classes following Command Pattern)
///   
/// Business Logic Layer:
///   - Services (Auth, Product, Cart, Order, Payment, Review, Report, Persistence)
///   - Factories (UserFactory, MenuFactory - Factory Pattern)
///   - Strategies (Report generation strategies - Strategy Pattern)
///   
/// Data Layer:
///   - AppDataStore (Singleton pattern for in-memory data)
///   - PersistenceService (JSON file-based storage)
///   - Models (User, Product, Order, Cart, etc.)
///   
/// Cross-Cutting:
///   - Validators (Centralized validation logic)
///   - Helpers (Console, Input, Validation, Display utilities)
///   
/// Design Patterns Used:
///   1. Factory Pattern - User and Menu creation
///   2. Strategy Pattern - Report generation algorithms
///   3. Command Pattern - Menu action encapsulation
///   4. Singleton Pattern - AppDataStore
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        // Configure all application services using ServiceConfigurator
        var services = ServiceConfigurator.ConfigureServices();

        // Initialize data: Load from JSON or use seed data
        InitializeData(services.PersistenceService);

        // Start the main menu with all configured services
        var mainMenu = new MainMenu(
            services.AuthService,
            services.ProductService,
            services.CartService,
            services.OrderService,
            services.PaymentService,
            services.ReviewService,
            services.ReportService,
            services.PersistenceService);
        
        mainMenu.Show();

        // Save all data before application exit
        services.PersistenceService.SaveData();
    }

    /// <summary>
    /// Initialize application data from storage or seed data
    /// </summary>
    private static void InitializeData(IPersistenceService persistenceService)
    {
        try
        {
            persistenceService.LoadData();

            // If no users exist, initialize with seed data
            if (!AppDataStore.Instance.Users.Any())
            {
                SeedData.Initialize();
                AppDataStore.Instance.SyncCountersWithData();
                Console.WriteLine("Initialized with seed data.");
            }
            else
            {
                Console.WriteLine("Loaded existing data from storage.");
            }
        }
        catch
        {
            // If loading fails (files don't exist), initialize with seed data
            SeedData.Initialize();
            AppDataStore.Instance.SyncCountersWithData();
            Console.WriteLine("Initialized with seed data.");
        }
    }
}
