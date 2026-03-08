using System.Text.Json;
using System.Text.Json.Serialization;
using OnlineShoppingSystem.Data;
using OnlineShoppingSystem.Enums;
using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Services;

/// <summary>
/// Service for saving and loading data to/from JSON files
/// </summary>
public class PersistenceService : IPersistenceService
{
    private readonly AppDataStore _dataStore;
    private readonly string _dataDirectory;
    private readonly JsonSerializerOptions _jsonOptions;

    public PersistenceService()
    {
        _dataStore = AppDataStore.Instance;
        _dataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Storage");

        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never,
            IncludeFields = false
        };

        EnsureDataDirectoryExists();
    }

    public void SaveData()
    {
        try
        {
            SaveToFile("users.json", _dataStore.Users);
            SaveToFile("products.json", _dataStore.Products);
            SaveToFile("carts.json", _dataStore.Carts);
            SaveToFile("orders.json", _dataStore.Orders);
            SaveToFile("payments.json", _dataStore.Payments);
            SaveToFile("reviews.json", _dataStore.Reviews);

            Console.WriteLine("Data saved successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving data: {ex.Message}");
        }
    }

    public void LoadData()
    {
        _dataStore.Users.Clear();
        
        // Load users with proper type handling
        var usersFromFile = LoadFromFileWithTypes("users.json");
        _dataStore.Users.AddRange(usersFromFile);

        _dataStore.Products.Clear();
        _dataStore.Products.AddRange(LoadFromFile<Product>("products.json"));

        _dataStore.Carts.Clear();
        _dataStore.Carts.AddRange(LoadFromFile<Cart>("carts.json"));

        _dataStore.Orders.Clear();
        _dataStore.Orders.AddRange(LoadFromFile<Order>("orders.json"));

        _dataStore.Payments.Clear();
        _dataStore.Payments.AddRange(LoadFromFile<Payment>("payments.json"));

        _dataStore.Reviews.Clear();
        _dataStore.Reviews.AddRange(LoadFromFile<Review>("reviews.json"));
    }

    /// <summary>
    /// Load users from JSON with proper type handling for Customer and Administrator
    /// </summary>
    private List<User> LoadFromFileWithTypes(string fileName)
    {
        var filePath = Path.Combine(_dataDirectory, fileName);

        if (!File.Exists(filePath))
        {
            return new List<User>();
        }

        var json = File.ReadAllText(filePath);
        
        // Parse as JsonDocument to check the Role property
        using var doc = JsonDocument.Parse(json);
        var users = new List<User>();

        foreach (var element in doc.RootElement.EnumerateArray())
        {
            var role = element.GetProperty("Role").GetInt32();
            
            if (role == (int)UserRole.Administrator)
            {
                var admin = JsonSerializer.Deserialize<Administrator>(element.GetRawText(), _jsonOptions);
                if (admin != null)
                {
                    users.Add(admin);
                }
            }
            else
            {
                var customer = JsonSerializer.Deserialize<Customer>(element.GetRawText(), _jsonOptions);
                if (customer != null)
                {
                    users.Add(customer);
                }
            }
        }

        return users;
    }

    private void SaveToFile<T>(string fileName, List<T> data)
    {
        var filePath = Path.Combine(_dataDirectory, fileName);
        var json = JsonSerializer.Serialize(data, _jsonOptions);
        File.WriteAllText(filePath, json);
    }

    private List<T> LoadFromFile<T>(string fileName)
    {
        var filePath = Path.Combine(_dataDirectory, fileName);

        if (!File.Exists(filePath))
        {
            return new List<T>();
        }

        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<T>>(json, _jsonOptions) ?? new List<T>();
    }

    private void EnsureDataDirectoryExists()
    {
        if (!Directory.Exists(_dataDirectory))
        {
            Directory.CreateDirectory(_dataDirectory);
        }
    }
}
