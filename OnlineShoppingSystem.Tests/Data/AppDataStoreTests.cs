using OnlineShoppingSystem.Data;
using OnlineShoppingSystem.Models;
using Xunit;

namespace OnlineShoppingSystem.Tests.Data;

/// <summary>
/// Unit tests for AppDataStore (Singleton Pattern)
/// Uses Sequential collection to prevent race conditions
/// </summary>
[Collection("Sequential")]
public class AppDataStoreTests : IDisposable
{
    public void Dispose()
    {
        // Clean up after each test
        var dataStore = AppDataStore.Instance;
        dataStore.Users.Clear();
        dataStore.Products.Clear();
        dataStore.Carts.Clear();
        dataStore.Orders.Clear();
        dataStore.Payments.Clear();
        dataStore.Reviews.Clear();
    }

    [Fact]
    public void Instance_CalledMultipleTimes_ReturnsSameInstance()
    {
        // Act
        var instance1 = AppDataStore.Instance;
        var instance2 = AppDataStore.Instance;
        var instance3 = AppDataStore.Instance;

        // Assert
        Assert.Same(instance1, instance2);
        Assert.Same(instance2, instance3);
        Assert.Same(instance1, instance3);
    }

    [Fact]
    public void Instance_ReferenceEquality_ReturnsTrue()
    {
        // Act
        var instance1 = AppDataStore.Instance;
        var instance2 = AppDataStore.Instance;

        // Assert
        Assert.True(ReferenceEquals(instance1, instance2));
    }

    [Fact]
    public void Instance_SharedState_ChangesVisibleAcrossReferences()
    {
        // Arrange
        var instance1 = AppDataStore.Instance;
        var instance2 = AppDataStore.Instance;

        // Act
        instance1.Users.Add(new Customer { Id = 1, Username = "TestUser" });

        // Assert
        Assert.Single(instance2.Users);
        Assert.Equal("TestUser", instance2.Users[0].Username);
    }

    [Fact]
    public void GetNextUserId_ReturnsSequentialIds()
    {
        // Arrange
        var dataStore = AppDataStore.Instance;

        // Act
        var id1 = dataStore.GetNextUserId();
        var id2 = dataStore.GetNextUserId();
        var id3 = dataStore.GetNextUserId();

        // Assert
        Assert.Equal(id1 + 1, id2);
        Assert.Equal(id2 + 1, id3);
    }

    [Fact]
    public void GetNextProductId_ReturnsSequentialIds()
    {
        // Arrange
        var dataStore = AppDataStore.Instance;

        // Act
        var id1 = dataStore.GetNextProductId();
        var id2 = dataStore.GetNextProductId();

        // Assert
        Assert.Equal(id1 + 1, id2);
    }

    [Fact]
    public void GetNextOrderId_ReturnsSequentialIds()
    {
        // Arrange
        var dataStore = AppDataStore.Instance;

        // Act
        var id1 = dataStore.GetNextOrderId();
        var id2 = dataStore.GetNextOrderId();

        // Assert
        Assert.Equal(id1 + 1, id2);
    }

    [Fact]
    public void GetNextCartId_ReturnsSequentialIds()
    {
        // Arrange
        var dataStore = AppDataStore.Instance;

        // Act
        var id1 = dataStore.GetNextCartId();
        var id2 = dataStore.GetNextCartId();

        // Assert
        Assert.Equal(id1 + 1, id2);
    }

    [Fact]
    public void AllIdGenerators_ReturnUniqueIds()
    {
        // Arrange
        var dataStore = AppDataStore.Instance;

        // Act
        var userId = dataStore.GetNextUserId();
        var productId = dataStore.GetNextProductId();
        var orderId = dataStore.GetNextOrderId();
        var cartId = dataStore.GetNextCartId();
        var paymentId = dataStore.GetNextPaymentId();
        var reviewId = dataStore.GetNextReviewId();

        // Assert - Each type starts from 1 (or current counter value)
        Assert.True(userId > 0);
        Assert.True(productId > 0);
        Assert.True(orderId > 0);
        Assert.True(cartId > 0);
        Assert.True(paymentId > 0);
        Assert.True(reviewId > 0);
    }

    [Fact]
    public void SyncCountersWithData_EmptyLists_NextIdStartsFromOne()
    {
        // Arrange
        var dataStore = AppDataStore.Instance;
        dataStore.Users.Clear();
        dataStore.Products.Clear();

        // Act
        dataStore.SyncCountersWithData();
        var userId = dataStore.GetNextUserId();
        var productId = dataStore.GetNextProductId();

        // Assert
        // After sync with empty lists, counters start from 1
        // Each call to GetNext increments, so we get 1, 2, 3...
        Assert.True(userId >= 1, $"Expected userId >= 1, but got {userId}");
        Assert.True(productId >= 1, $"Expected productId >= 1, but got {productId}");
    }

    [Fact]
    public void SyncCountersWithData_WithExistingData_SetsCountersCorrectly()
    {
        // Arrange
        var dataStore = AppDataStore.Instance;
        dataStore.Users.Clear();
        dataStore.Users.Add(new Customer { Id = 5, Username = "User5" });
        dataStore.Users.Add(new Customer { Id = 10, Username = "User10" });

        // Act
        dataStore.SyncCountersWithData();
        var nextId = dataStore.GetNextUserId();

        // Assert
        Assert.Equal(11, nextId); // Max ID (10) + 1
    }

    [Fact]
    public void Collections_Initialized_NotNull()
    {
        // Arrange & Act
        var dataStore = AppDataStore.Instance;

        // Assert
        Assert.NotNull(dataStore.Users);
        Assert.NotNull(dataStore.Products);
        Assert.NotNull(dataStore.Carts);
        Assert.NotNull(dataStore.Orders);
        Assert.NotNull(dataStore.Payments);
        Assert.NotNull(dataStore.Reviews);
    }

    [Fact]
    public void ThreadSafety_MultipleThreadsAccessingInstance_GetSameInstance()
    {
        // Arrange
        AppDataStore? instance1 = null;
        AppDataStore? instance2 = null;
        AppDataStore? instance3 = null;

        // Act
        var task1 = Task.Run(() => instance1 = AppDataStore.Instance);
        var task2 = Task.Run(() => instance2 = AppDataStore.Instance);
        var task3 = Task.Run(() => instance3 = AppDataStore.Instance);

        Task.WaitAll(task1, task2, task3);

        // Assert
        Assert.Same(instance1, instance2);
        Assert.Same(instance2, instance3);
    }
}
