using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Data;

/// <summary>
/// Central in-memory data store for all application data
/// </summary>
public class AppDataStore
{
    private static AppDataStore? _instance;
    private static readonly object _lock = new object();

    public List<User> Users { get; set; }
    public List<Product> Products { get; set; }
    public List<Cart> Carts { get; set; }
    public List<Order> Orders { get; set; }
    public List<Payment> Payments { get; set; }
    public List<Review> Reviews { get; set; }

    private int _nextUserId;
    private int _nextProductId;
    private int _nextCartId;
    private int _nextOrderId;
    private int _nextPaymentId;
    private int _nextReviewId;

    private AppDataStore()
    {
        Users = new List<User>();
        Products = new List<Product>();
        Carts = new List<Cart>();
        Orders = new List<Order>();
        Payments = new List<Payment>();
        Reviews = new List<Review>();

        _nextUserId = 1;
        _nextProductId = 1;
        _nextCartId = 1;
        _nextOrderId = 1;
        _nextPaymentId = 1;
        _nextReviewId = 1;
    }

    /// <summary>
    /// Get the singleton instance of the data store
    /// </summary>
    public static AppDataStore Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new AppDataStore();
                    }
                }
            }
            return _instance;
        }
    }

    public int GetNextUserId() => _nextUserId++;
    public int GetNextProductId() => _nextProductId++;
    public int GetNextCartId() => _nextCartId++;
    public int GetNextOrderId() => _nextOrderId++;
    public int GetNextPaymentId() => _nextPaymentId++;
    public int GetNextReviewId() => _nextReviewId++;

    /// <summary>
    /// Reset all data and counters (useful for testing)
    /// </summary>
    public void Reset()
    {
        Users.Clear();
        Products.Clear();
        Carts.Clear();
        Orders.Clear();
        Payments.Clear();
        Reviews.Clear();

        _nextUserId = 1;
        _nextProductId = 1;
        _nextCartId = 1;
        _nextOrderId = 1;
        _nextPaymentId = 1;
        _nextReviewId = 1;
    }
}
