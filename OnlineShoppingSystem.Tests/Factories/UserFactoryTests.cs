using OnlineShoppingSystem.Data;
using OnlineShoppingSystem.Enums;
using OnlineShoppingSystem.Factories;
using OnlineShoppingSystem.Models;
using Xunit;

namespace OnlineShoppingSystem.Tests.Factories;

/// <summary>
/// Unit tests for UserFactory (Factory Pattern)
/// Uses Sequential collection to prevent race conditions with singleton AppDataStore
/// </summary>
[Collection("Sequential")]
public class UserFactoryTests : IDisposable
{
    public UserFactoryTests()
    {
        // Reset singleton before each test
        ResetAppDataStore();
    }

    public void Dispose()
    {
        // Clean up after each test
        ResetAppDataStore();
    }

    private void ResetAppDataStore()
    {
        var dataStore = AppDataStore.Instance;
        dataStore.Users.Clear();
        dataStore.Products.Clear();
        dataStore.Carts.Clear();
        dataStore.Orders.Clear();
        dataStore.Payments.Clear();
        dataStore.Reviews.Clear();
    }

    [Fact]
    public void CreateUser_CustomerRole_ReturnsCustomer()
    {
        // Arrange
        var username = "testuser";
        var email = "test@example.com";
        var password = "password123";
        var role = UserRole.Customer;

        // Act
        var user = UserFactory.CreateUser(username, email, password, role);

        // Assert
        Assert.NotNull(user);
        Assert.IsType<Customer>(user);
        Assert.Equal(username, user.Username);
        Assert.Equal(email.ToLower(), user.Email);
        Assert.Equal(password, user.Password);
        Assert.Equal(role, user.Role);
    }

    [Fact]
    public void CreateUser_AdministratorRole_ReturnsAdministrator()
    {
        // Arrange
        var username = "adminuser";
        var email = "admin@example.com";
        var password = "admin123";
        var role = UserRole.Administrator;

        // Act
        var user = UserFactory.CreateUser(username, email, password, role);

        // Assert
        Assert.NotNull(user);
        Assert.IsType<Administrator>(user);
        Assert.Equal(username, user.Username);
        Assert.Equal(email.ToLower(), user.Email);
        Assert.Equal(role, user.Role);
    }

    [Fact]
    public void CreateUser_Customer_InitializesWithZeroBalance()
    {
        // Arrange
        var role = UserRole.Customer;

        // Act
        var user = UserFactory.CreateUser("user", "user@test.com", "pass", role);
        var customer = user as Customer;

        // Assert
        Assert.NotNull(customer);
        Assert.Equal(0, customer.WalletBalance);
        Assert.NotNull(customer.OrderIds);
        Assert.Empty(customer.OrderIds);
    }

    [Fact]
    public void CreateUser_TrimsUsernameAndEmail()
    {
        // Arrange
        var username = "  testuser  ";
        var email = "  TEST@EXAMPLE.COM  ";
        var role = UserRole.Customer;

        // Act
        var user = UserFactory.CreateUser(username, email, "pass", role);

        // Assert
        Assert.Equal("testuser", user.Username);
        Assert.Equal("test@example.com", user.Email);
    }

    [Fact]
    public void CreateUser_SetsCreatedAtTimestamp()
    {
        // Arrange
        var beforeCreation = DateTime.Now.AddSeconds(-1);

        // Act
        var user = UserFactory.CreateUser("user", "user@test.com", "pass", UserRole.Customer);
        var afterCreation = DateTime.Now.AddSeconds(1);

        // Assert
        Assert.True(user.CreatedAt >= beforeCreation);
        Assert.True(user.CreatedAt <= afterCreation);
    }

    [Fact]
    public void CreateUser_GeneratesUniqueIds()
    {
        // Act
        var user1 = UserFactory.CreateUser("user1", "user1@test.com", "pass", UserRole.Customer);
        var user2 = UserFactory.CreateUser("user2", "user2@test.com", "pass", UserRole.Customer);

        // Assert
        Assert.NotEqual(user1.Id, user2.Id);
        Assert.True(user2.Id > user1.Id);
    }

    [Fact]
    public void CreateCustomerWithBalance_SetsInitialBalance()
    {
        // Arrange
        var initialBalance = 1000m;

        // Act
        var customer = UserFactory.CreateCustomerWithBalance(
            "user", "user@test.com", "pass", initialBalance);

        // Assert
        Assert.Equal(initialBalance, customer.WalletBalance);
        Assert.IsType<Customer>(customer);
    }

    [Theory]
    [InlineData(UserRole.Customer)]
    [InlineData(UserRole.Administrator)]
    public void CreateUser_DifferentRoles_ReturnsCorrectType(UserRole role)
    {
        // Act
        var user = UserFactory.CreateUser("user", "user@test.com", "pass", role);

        // Assert
        Assert.Equal(role, user.Role);
        
        if (role == UserRole.Customer)
        {
            Assert.IsType<Customer>(user);
        }
        else
        {
            Assert.IsType<Administrator>(user);
        }
    }

    [Fact]
    public void CreateUser_MultipleCustomers_AllHaveEmptyOrderLists()
    {
        // Act
        var customer1 = UserFactory.CreateUser("u1", "u1@test.com", "p", UserRole.Customer) as Customer;
        var customer2 = UserFactory.CreateUser("u2", "u2@test.com", "p", UserRole.Customer) as Customer;

        // Assert
        Assert.NotNull(customer1?.OrderIds);
        Assert.NotNull(customer2?.OrderIds);
        Assert.Empty(customer1.OrderIds);
        Assert.Empty(customer2.OrderIds);
    }
}
